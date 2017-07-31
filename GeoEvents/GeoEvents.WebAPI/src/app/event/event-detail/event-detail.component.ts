import { Component, ElementRef, EventEmitter, Input, NgZone, OnInit, Output, ViewChild } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { ActivatedRoute } from "@angular/router";
import { FormGroup } from "@angular/forms";
import { MapsAPILoader } from "@agm/core";
import { Observable } from "rxjs/Observable";

import { Event } from "../shared/models/event.model";
import { Image } from "../shared/models/image.model";
import { Location } from "../shared/models/location.model";
import { EventService } from "../shared/event.service";
import { CategoryService } from "../shared/category.service";
import { LoaderService } from "../../shared/loader.service";
import { LocationService } from "../shared/location.service";

@Component({
    selector: "event-details",
    styleUrls: ["app/event/event-detail/event-detail.component.css"],
    templateUrl: "app/event/event-detail/event-detail.component.html"
})

export class EventDetailComponent implements OnInit {
    @ViewChild("carousel") carouselElement: ElementRef;
    eventForm: FormGroup;
    hasRated: boolean = false;
    rating: number;

    private event: Event;
    private getImagesLoading: boolean = false;
    private images: Image[];
    private location: Location;

    constructor(
        private activatedRoute: ActivatedRoute,
        private categoryService: CategoryService,
        private eventService: EventService,
        private loaderService: LoaderService,
        private locationService: LocationService,
        private mapsAPILoader: MapsAPILoader,
        private ngZone: NgZone
    ) {

    }

    decodeBase64(s: string) {
        var e = {}, i, b = 0, c, x, l = 0, a, r = "", w = String.fromCharCode, L = s.length;
        var A = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        for (i = 0; i < 64; i++) { e[A.charAt(i)] = i; }
        for (x = 0; x < L; x++) {
            c = e[s.charAt(x)]; b = (b << 6) + c; l += 6;
            while (l >= 8) { ((a = (b >>> (l -= 8)) & 0xff) || (x < (L - 2))) && (r += w(a)); }
        }
        return r;
    }

    ngOnInit() {
        this.event = this.activatedRoute.snapshot.data.event;
        this.event.Custom = eval(this.event.Custom);

        this.categoryService.buildCategories('category', false);

        this.locationService.getLocationById(this.event.LocationId).subscribe((response: Location) => {
            this.location = response;
        })

        this.loaderService.loaderStatus.subscribe((value: boolean) => {
            this.getImagesLoading = value;
        });

        this.loaderService.displayLoader(true);

        this.eventService.getImages(this.activatedRoute.snapshot.params["eventId"]).subscribe((res: Image[]) => {
            this.images = res
            for (var i = 0; i < this.images.length; i++) {
                var item = document.createElement("div");

                if (i == 0) item.setAttribute("class", "item active");
                else item.setAttribute("class", "item");

                if (this.images[i].Content.substr(0, 10) != "PD94bWwgdm") {
                    var img = document.createElement("img");
                    img.setAttribute("src", "data:image/jpeg;base64," + this.images[i].Content);

                    item.appendChild(img);
                } else {
                    var svg = this.parseSvg(this.decodeBase64(this.images[i].Content));

                    item.appendChild(svg);
                }

                this.carouselElement.nativeElement.appendChild(item);
            }
            this.loaderService.displayLoader(false);
        });
    }

    parseSvg(xmlString: string) {
        let parser = new DOMParser();
        let doc = parser.parseFromString(xmlString, "image/svg+xml");
        return doc.documentElement;
    }

    rate(rating: number) {
        if (!this.hasRated){
            this.eventService.updateRating(this.event.Id, +rating, this.event.Rating, this.event.RateCount)
                .subscribe((response: Event) => {
                    this.event.Rating = response.Rating;
                    this.event.RateCount = response.RateCount;
                    this.rating = rating;
                    this.hasRated = true;
                });
            this.locationService.updateRating(this.location.Id, +rating, this.location.Rating, this.location.RateCount)
                .subscribe((response: Location) => {
                    this.location.Rating = response.Rating;
                    this.location.RateCount = response.RateCount;
                });
        }
    }

    reserve() {
        this.eventService.updateReservation(this.event.Id)
            .subscribe((response: Event) => {
                this.event.Reserved = response.Reserved;
            });
    }    
}