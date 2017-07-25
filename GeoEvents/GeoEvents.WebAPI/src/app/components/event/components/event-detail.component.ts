import { Component, Input, OnInit, NgZone, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { MapsAPILoader } from '@agm/core';
import { FormGroup } from '@angular/forms';

import { IEvent } from '../models/event.model';
import { IImage } from '../models/image.model';
import { ILocation } from '../models/location.model';

import { EventService } from '../providers/event.service';
import { LoaderService } from '../../../shared/loader.service';
import { CategoryService } from '../providers/category.service';
import { LocationService } from '../providers/location.service';

@Component({
    templateUrl: 'app/components/event/views/event-detail.component.html',
    selector: 'event-details',
    styleUrls: ['app/components/event/views/event-detail.component.css']
})

export class EventDetailComponent implements OnInit {
    private _event: IEvent;
    private _images: IImage[];
    private _location: ILocation;
    @ViewChild("carousel") carouselElement: ElementRef;
    @ViewChild("userRate") userRateElement: ElementRef;
    @ViewChild("search") searchElementRef: ElementRef;
    private _getImagesLoading: boolean = false;

    rating: number;
    hasRated: boolean = false;
    eventForm: FormGroup;

    constructor(
        private _mapsAPILoader: MapsAPILoader,
        private _ngZone: NgZone,
        private _eventService: EventService,
        private _loaderService: LoaderService,
        private _activatedRoute: ActivatedRoute,
        private _categoryService: CategoryService,
        private _locationService: LocationService
    ) {

    }

    ngOnInit() {
        this.event = this._activatedRoute.snapshot.data.event;
        this.event.Custom = eval(this.event.Custom);

        this._categoryService.buildCategories();

        this._locationService.getLocationById(this.event.LocationId).subscribe((response: ILocation) => {
            this.location = response;
        })

        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.getImagesLoading = value;
        });

        this._loaderService.displayLoader(true);

        this._eventService.getImages(this._activatedRoute.snapshot.params['eventId']).subscribe((res: IImage[]) => {
            this.images = res
            for (var i = 0; i < this.images.length; i++) {
                var item = document.createElement("div");

                if (i == 0) item.setAttribute("class", "item active");
                else item.setAttribute("class", "item");

                if (this.images[i].Content.substr(0, 10) != 'PD94bWwgdm') {
                    var img = document.createElement("img");
                    img.setAttribute("src", "data:image/jpeg;base64," + this.images[i].Content);

                    item.appendChild(img);
                } else {
                    var svg = this.parseSvg(this.decodeBase64(this.images[i].Content));

                    item.appendChild(svg);
                }

                this.carouselElement.nativeElement.appendChild(item);
            }
            this._loaderService.displayLoader(false);
        });
    }

    rateChange(slider: any) {
        this.userRateElement.nativeElement.innerHTML = slider.value;
    }

    rate(rating: number) {
        if (!this.hasRated){
            this._eventService.updateRating(this.event.Id, +rating, this.event.Rating, this.event.RateCount)
                .subscribe((response: IEvent) => {
                    this.event.Rating = response.Rating;
                    this.event.RateCount = response.RateCount;
                    this.rating = rating;
                    this.hasRated = true;
                });
            this._locationService.updateRating(this.location.Id, +rating, this.location.Rating, this.location.RateCount)
                .subscribe((response: ILocation) => {
                    this.location.Rating = response.Rating;
                    this.location.RateCount = response.RateCount;
                });
        }

    }

    reserve() {
        this._eventService.updateReservation(this.event.Id)
            .subscribe((response: IEvent) => {
                this.event.Reserved = response.Reserved;
            });
    }

    decodeBase64(s: string) {
        var e = {}, i, b = 0, c, x, l = 0, a, r = '', w = String.fromCharCode, L = s.length;
        var A = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        for (i = 0; i < 64; i++) { e[A.charAt(i)] = i; }
        for (x = 0; x < L; x++) {
            c = e[s.charAt(x)]; b = (b << 6) + c; l += 6;
            while (l >= 8) { ((a = (b >>> (l -= 8)) & 0xff) || (x < (L - 2))) && (r += w(a)); }
        }
        return r;
    }

    parseSvg(xmlString: string) {
        let parser = new DOMParser();
        let doc = parser.parseFromString(xmlString, "image/svg+xml");
        return doc.documentElement;
    }

    get event(): IEvent {
        return this._event;
    }

    set event(thisEvent: IEvent) {
        this._event = thisEvent;
    }

    get getImagesLoading(): boolean {
        return this._getImagesLoading;
    }

    set getImagesLoading(thisGetImagesLoading: boolean) {
        this._getImagesLoading = thisGetImagesLoading;
    }

    get images() : IImage[] {
        return this._images;
    }

    set images(theImages: IImage[]) {
        this._images = theImages;
    }

    get location(): ILocation {
        return this._location;
    }

    set location(theLocation: ILocation) {
        this._location = theLocation;
    }
}