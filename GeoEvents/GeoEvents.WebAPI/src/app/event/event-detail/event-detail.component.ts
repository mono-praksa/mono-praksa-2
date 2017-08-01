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
    eventFirstDay: number = -1;
    hasRated: boolean = false;
    rating: number;
    eventRepetition: number = 0;

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

    moveDate(movement: number) {
        this.eventRepetition += movement;
        let startTime = new Date(this.event.StartTime);
        let endTime = new Date(this.event.EndTime);

        if (this.event.Occurrence == 'daily') {
            startTime.setDate(startTime.getDate() + movement * this.event.RepeatEvery);
            endTime.setDate(endTime.getDate() + movement * this.event.RepeatEvery);
        } else if (this.event.Occurrence == 'weekly') {
            let extremeNumberExceptInfinity = function (list: number[], movement: number) {
                list.sort();

                if (movement == 1) { // search minimum
                    if (list[0] == -Infinity) {
                        return list[1];
                    } else {
                        return list[0];
                    }
                } else if (movement == -1) { // search maximum
                    if (list[list.length - 1] == Infinity) {
                        return list[list.length - 2];
                    } else {
                        return list[list.length - 1];
                    }
                }
            }
            let listOfDays = this.event.RepeatOnList.map((n: number) => Math.log2(n)); // 0 - sun, 1 - mon, etc
            listOfDays.push(movement * Infinity);
            listOfDays.sort((a: number, b: number) => (movement * (a - b)))

            if (this.eventFirstDay == -1) {
                this.eventFirstDay = startTime.getDay();
            }

            let moveDay;
            
            for (let i = 0; i < listOfDays.length; i++) {
                if (movement * listOfDays[i] >= movement * startTime.getDay()) {
                    if (movement * listOfDays[i] > movement * startTime.getDay()) {
                        moveDay = listOfDays[i];
                    } else {
                        moveDay = listOfDays[i + 1];
                    }
                    break;
                }
            }

            if (this.eventRepetition == 0) {
                moveDay = this.eventFirstDay;
            }

            
            if (moveDay == movement * Infinity) {
                let value: number = movement * this.event.RepeatEvery * 7 + extremeNumberExceptInfinity(listOfDays, movement) - startTime.getDay();

                startTime.setDate(startTime.getDate() + value);
                endTime.setDate(endTime.getDate() + value);
            } else {
                startTime.setDate(startTime.getDate() + moveDay - startTime.getDay());
                endTime.setDate(endTime.getDate() + moveDay - endTime.getDay());
            }

        } else if (this.event.Occurrence == 'monthly') {
            let startDay = startTime.getDate();
            let endDay = endTime.getDate();

            let moveStartTime = new Date(startTime.getFullYear(), startTime.getMonth() + movement * this.event.RepeatEvery, 1);
            let moveEndTime = new Date(endTime.getFullYear(), endTime.getMonth() + movement * this.event.RepeatEvery, 1);

            let moveStartTimeNumberOfDays = new Date(moveStartTime.getFullYear(), moveStartTime.getMonth() + 1, 0).getDate();
            let moveEndTimeNumberOfDays = new Date(moveEndTime.getFullYear(), moveEndTime.getMonth() + 1, 0).getDate();
            
            while (startDay > moveStartTimeNumberOfDays || endDay > moveEndTimeNumberOfDays) {
                moveStartTime = new Date(moveStartTime.getFullYear(), moveStartTime.getMonth() + movement * this.event.RepeatEvery, 1);
                moveEndTime = new Date(moveEndTime.getFullYear(), moveEndTime.getMonth() + movement * this.event.RepeatEvery, 1);

                moveStartTimeNumberOfDays = new Date(moveStartTime.getFullYear(), moveStartTime.getMonth() + 1, 0).getDate();
                moveEndTimeNumberOfDays = new Date(moveEndTime.getFullYear(), moveEndTime.getMonth() + 1, 0).getDate();
            }

            moveStartTime.setDate(startDay);
            moveEndTime.setDate(endDay);

            moveStartTime.setHours(startTime.getHours());
            moveEndTime.setHours(endTime.getHours());

            moveStartTime.setMinutes(startTime.getMinutes());
            moveEndTime.setMinutes(endTime.getMinutes());
            
            startTime = moveStartTime;
            endTime = moveEndTime;
        } else if (this.event.Occurrence == 'yearly') {
            startTime.setFullYear(startTime.getFullYear() + movement * this.event.RepeatEvery);
            endTime.setFullYear(endTime.getFullYear() + movement * this.event.RepeatEvery);
        }

        this.event.StartTime = startTime;
        this.event.EndTime = endTime;
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