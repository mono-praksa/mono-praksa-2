import { Component, ElementRef, EventEmitter, Input, NgZone, OnInit, Output, ViewChild } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { ActivatedRoute } from "@angular/router";
import { FormGroup } from "@angular/forms";
import { MapsAPILoader } from "@agm/core";
import { Observable } from "rxjs/Observable";

import { CategoryService } from "../shared/category.service";
import { Event } from "../shared/models/event.model";
import { EventService } from "../shared/event.service";
import { Image } from "../shared/models/image.model";
import { ImageService } from "../shared/image.service";
import { Location } from "../shared/models/location.model";
import { LoaderService } from "../../shared/loader.service";
import { LocationService } from "../shared/location.service";

@Component({
    selector: "event-details",
    styleUrls: ["app/event/event-detail/event-detail.component.css"],
    templateUrl: "app/event/event-detail/event-detail.component.html"
})

export class EventDetailComponent implements OnInit {
    @ViewChild("carousel") carouselElement: ElementRef;
    dates: { start: Date, end: Date }[] = [];
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
        private imageService: ImageService,
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

        if (movement == -1) {
            let startTime = new Date(this.dates[this.eventRepetition].start);
            let endTime = new Date(this.dates[this.eventRepetition].end);
            
            this.event.StartTime = startTime;
            this.event.EndTime = endTime;
        } else if (movement == 1) {
            let startTime: Date;
            let endTime: Date;

            if (this.eventRepetition < this.dates.length) {
                startTime = this.dates[this.eventRepetition].start;
                endTime = this.dates[this.eventRepetition].end;
                
                this.event.StartTime = startTime;
                this.event.EndTime = endTime;
            } else {
                startTime = new Date(this.event.StartTime);
                endTime = new Date(this.event.EndTime);

                if (this.event.Occurrence == 'daily') {
                    startTime.setDate(startTime.getDate() + this.event.RepeatEvery);
                    endTime.setDate(endTime.getDate() + this.event.RepeatEvery);
                } else if (this.event.Occurrence == 'weekly') {
                    let listOfDays = this.event.RepeatOnList.map((n: number) => Math.log2(n)); // 0 - sun, 1 - mon, etc
                    listOfDays.push(Infinity);

                    let moveDay;

                    for (let i = 0; i < listOfDays.length; i++) {
                        if (listOfDays[i] >= startTime.getDay()) {
                            if (listOfDays[i] > startTime.getDay()) {
                                moveDay = listOfDays[i];
                            } else {
                                moveDay = listOfDays[i + 1];
                            }
                            break;
                        }
                    }


                    if (moveDay == Infinity) {
                        let value: number = this.event.RepeatEvery * 7 + listOfDays[0] - startTime.getDay();

                        startTime.setDate(startTime.getDate() + value);
                        endTime.setDate(endTime.getDate() + value);
                    } else {
                        startTime.setDate(startTime.getDate() + moveDay - startTime.getDay());
                        endTime.setDate(endTime.getDate() + moveDay - endTime.getDay());
                    }

                } else if (this.event.Occurrence == 'monthly') {
                    let startDay = startTime.getDate();
                    let endDay = endTime.getDate();

                    let moveStartTime = new Date(startTime.getFullYear(), startTime.getMonth() + this.event.RepeatEvery, 1);
                    let moveEndTime = new Date(endTime.getFullYear(), endTime.getMonth() + this.event.RepeatEvery, 1);

                    let moveStartTimeNumberOfDays = new Date(moveStartTime.getFullYear(), moveStartTime.getMonth() + 1, 0).getDate();
                    let moveEndTimeNumberOfDays = new Date(moveEndTime.getFullYear(), moveEndTime.getMonth() + 1, 0).getDate();

                    while (startDay > moveStartTimeNumberOfDays || endDay > moveEndTimeNumberOfDays) {
                        moveStartTime = new Date(moveStartTime.getFullYear(), moveStartTime.getMonth() + this.event.RepeatEvery, 1);
                        moveEndTime = new Date(moveEndTime.getFullYear(), moveEndTime.getMonth() + this.event.RepeatEvery, 1);

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
                } else if (this.event.Occurrence == 'monthlydayname') {
                    let startWeek = getWeekOfMonth(startTime);
                    let endWeek = getWeekOfMonth(endTime);

                    let startDay = startTime.getDay();
                    let differenceDay: number = Math.floor((+endTime - +startTime) / 1000 / 60 / 60 / 24);

                    let moveStartMonth = startTime.getMonth() + this.event.RepeatEvery;
                    let moveEndMonth = endTime.getMonth() + this.event.RepeatEvery;

                    let moveStartTime = new Date(startTime.getFullYear(), moveStartMonth, 1);
                    let moveEndTime = new Date(endTime.getFullYear(), moveEndMonth, 1);
                    
                    let moveStartDay;

                    if (moveStartTime.getDay() > startDay) {
                        moveStartDay = 8 - (moveStartTime.getDay() - startDay);
                    } else {
                        moveStartDay = 1 + startDay - moveStartTime.getDay();
                    }

                    moveStartTime.setDate(moveStartDay);
                    moveEndTime.setDate(moveStartDay + differenceDay);
                    
                    setWeekOfMonth(moveStartTime, startWeek);
                    setWeekOfMonth(moveEndTime, endWeek);
                    
                    while (moveStartTime.getMonth() != (moveStartMonth % 12) || moveEndTime.getMonth() != (moveEndMonth % 12)) {
                        moveStartMonth += this.event.RepeatEvery;
                        moveEndMonth += this.event.RepeatEvery;

                        moveStartTime = new Date(startTime.getFullYear(), moveStartMonth, 1);
                        moveEndTime = new Date(endTime.getFullYear(), moveEndMonth, 1);
                        
                        if (moveStartTime.getDay() > startDay) {
                            moveStartDay = 8 - (moveStartTime.getDay() - startDay);
                        } else {
                            moveStartDay = 1 + startDay - moveStartTime.getDay();
                        }

                        moveStartTime.setDate(moveStartDay);
                        moveEndTime.setDate(moveStartDay + differenceDay);
                        
                        setWeekOfMonth(moveStartTime, startWeek);
                        setWeekOfMonth(moveEndTime, endWeek);
                    }

                    moveStartTime.setHours(startTime.getHours());
                    moveEndTime.setHours(endTime.getHours());

                    moveStartTime.setMinutes(startTime.getMinutes());
                    moveEndTime.setMinutes(endTime.getMinutes());

                    startTime = setWeekOfMonth(moveStartTime, startWeek);
                    endTime = setWeekOfMonth(moveEndTime, endWeek);
                } else if (this.event.Occurrence == 'yearly') {
                    let moveStartTime = new Date(startTime.getFullYear() + this.event.RepeatEvery, startTime.getMonth(), 1);
                    let moveEndTime = new Date(endTime.getFullYear() + this.event.RepeatEvery, endTime.getMonth(), 1);

                    if ((startTime.getDate() == 29 && startTime.getMonth() == 1) || (endTime.getDate() == 29 && endTime.getMonth() == 1)) { // if feb29
                        while ((startTime.getDate() == 29 && startTime.getMonth() == 1 && !isLeapYear(moveStartTime.getFullYear())) ||
                            (endTime.getDate() == 29 && endTime.getMonth() == 1 && !isLeapYear(moveEndTime.getFullYear()))) {
                            moveStartTime = new Date(moveStartTime.getFullYear() + this.event.RepeatEvery, moveStartTime.getMonth(), 1);
                            moveEndTime = new Date(moveEndTime.getFullYear() + this.event.RepeatEvery, moveEndTime.getMonth(), 1);
                        }
                    }

                    moveStartTime.setDate(startTime.getDate());
                    moveEndTime.setDate(endTime.getDate());

                    moveStartTime.setHours(startTime.getHours());
                    moveEndTime.setHours(endTime.getHours());

                    moveStartTime.setMinutes(startTime.getMinutes());
                    moveEndTime.setMinutes(endTime.getMinutes());

                    startTime = moveStartTime;
                    endTime = moveEndTime;
                }

                this.event.StartTime = startTime;
                this.event.EndTime = endTime;
                this.dates.push({ start: startTime, end: endTime });
            }
        }
    }

    ngOnInit() {
        this.event = this.activatedRoute.snapshot.data.event;
        this.event.Custom = eval(this.event.Custom);

        this.dates.push({ start: this.event.StartTime, end: this.event.EndTime });

        this.categoryService.buildCategories('category', false);

        this.locationService.getLocationById(this.event.LocationId).subscribe((response: Location) => {
            this.location = response;
        })

        this.loaderService.loaderStatus.subscribe((value: boolean) => {
            this.getImagesLoading = value;
        });

        this.loaderService.displayLoader(true);

        this.imageService.getImages(this.activatedRoute.snapshot.params["eventId"]).subscribe((res: Image[]) => {
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

let getWeekOfMonth = function (date: Date, exact: boolean = true) {
    var month = date.getMonth(),
        year = date.getFullYear(),
        firstWeekday = new Date(year, month, 1).getDay(),
        lastDateOfMonth = new Date(year, month + 1, 0).getDate(),
        offsetDate = date.getDate() + firstWeekday - 1,
        index = 1,
        weeksInMonth = index + Math.ceil((lastDateOfMonth + firstWeekday - 7) / 7),
        week = index + Math.floor(offsetDate / 7);
    if (exact || week < 2 + index) return week;
    return week === weeksInMonth ? index + 5 : week;
};

function setWeekOfMonth (date: Date, week: number) {
    date.setDate(date.getDate() + (week - getWeekOfMonth(date)) * 7);
    return date;
}

function isLeapYear(year: number) {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}