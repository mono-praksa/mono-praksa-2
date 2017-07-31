import { MapsAPILoader } from "@agm/core";
import { Component, ElementRef, EventEmitter, NgZone, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { Observable } from "rxjs/Observable";

import { CategoryService } from "../../shared/category.service";
import { EventService } from "../../shared/event.service";
import { endDateBeforeStartDate, uniqueName } from "../../shared/validator";
import { LocationService } from "../../shared/location.service";

import { Event } from "../../shared/models/event.model";
import { Location } from "../../shared/models/location.model";

import { GeocodingService } from "../../../shared/geocoding.service";
import { LoaderService } from "../../../shared/loader.service";

@Component({
    selector: "create-event",
    templateUrl: "app/event/event-create/event-create-data/event-create-data.component.html",
    styleUrls: ["app/event/event-create/event-create-data/event-create-data.component.css"]
})
export class EventCreateDataComponent implements OnInit {
    @Output() eventEmitter = new EventEmitter();
    monthlyOption: string = "month";
    repeat: { valueType: string, value: string } = {valueType: "", value: ""};
    @ViewChild("search") searchElementRef: ElementRef;

    address: FormControl;
    capacity: FormControl;
    category: FormControl;
    description: FormControl;
    end: FormControl;
    eventForm: FormGroup;
    latitude: FormControl;
    longitude: FormControl;
    name: FormControl;
    occurrence: FormControl;
    price: FormControl;
    start: FormControl;

    // FormControls for recurring
    repeatEvery: FormControl;

    private createdEvent: Event;
    private createEventLoading: boolean = false;
    private isAddressValid: boolean = false;
    private zoom: number = 12;

    constructor(
        private categoryService: CategoryService,
        private geocodingService: GeocodingService,
        private loaderService: LoaderService,
        private locationService: LocationService,
        private mapsAPILoader: MapsAPILoader,
        private ngZone: NgZone
    ) { }

    ngOnInit(): void {
        this.loaderService.loaderStatus.subscribe((value: boolean) => {
            this.createEventLoading = value;
        });

        this.categoryService.buildCategories("category", true);
        this.categoryService.buildCategories("day");

        this.buildForm();

        //GOOGLE MAPS
        this.setCurrentPosition();

        this.mapsAPILoader.load().then(() => {
            let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
                types: ["address"]
            });
            autocomplete.addListener("place_changed", () => {
                this.ngZone.run(() => {
                    //get the place result
                    let place: google.maps.places.PlaceResult = autocomplete.getPlace();

                    //verify result
                    if (place.geometry === undefined || place.geometry === null) {
                        return;
                    }

                    //set latitude, longitude and zoom
                    this.eventForm.controls["latitude"].setValue(place.geometry.location.lat());
                    this.eventForm.controls["longitude"].setValue(place.geometry.location.lng());
                    this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
                        this.eventForm.controls["address"].setValue(response);
                    });
                    this.isAddressValid = true;
                });
            });
        });
    }

    private buildForm(): void {
        this.address = new FormControl("", Validators.required);
        this.capacity = new FormControl("", [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]);
        this.description = new FormControl("", Validators.required);
        this.end = new FormControl("", Validators.required);
        this.latitude = new FormControl("", Validators.required);
        this.longitude = new FormControl("", Validators.required);
        this.name = new FormControl("", Validators.required);
        this.occurrence = new FormControl("none");
        this.price = new FormControl("", [Validators.required, Validators.pattern(/^[0-9]+(\.\d{1,2})?$/)]);
        this.start = new FormControl("", Validators.required);

        // FormControls for reccuring
        this.repeatEvery = new FormControl(1);

        this.eventForm = new FormGroup({
            address: this.address,
            capacity: this.capacity,
            description: this.description,
            end: this.end,
            latitude: this.latitude,
            longitude: this.longitude,
            name: this.name,
            price: this.price,
            start: this.start,
            occurrence: this.occurrence,

            // FormControls for reccuring
            repeatEvery: this.repeatEvery
        }, endDateBeforeStartDate("start", "end"));
    }

    createEvent(formValues: any) {
        this.loaderService.displayLoader(true);
        let chosenCategories: number[] = [];
        let chosenDays: number[] = [];
        let repeatCount: number = 0;

        this.categoryService.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

        if (this.occurrence.value == "weekly") {
            this.categoryService.days.filter(checkbox => {
                if (checkbox.checked) {
                    chosenDays.push(checkbox.id);
                }
            });
        }

        if (this.occurrence.value != "none") {
            repeatCount = this.endOfRepeatingNumber();
        }

        let occurrence = formValues.occurrence;

        if (this.monthlyOption == "week") {
            occurrence += "dayname";
        }

        let newEvent : Event = {
            Capacity: formValues.capacity,
            Categories: chosenCategories,
            Custom: undefined,
            CustomModel: undefined,
            Description: formValues.description,
            EndTime: formValues.end,
            Name: formValues.name,
            Id: undefined,
            Latitude: formValues.latitude,
            LocationId: undefined,
            Longitude: formValues.longitude,
            Occurrence: occurrence,
            Price: formValues.price,
            RateCount: undefined,
            Rating: undefined,
            Reserved: undefined,
            StartTime: formValues.start,

            // attributes for reccuring events
            RepeatCount: repeatCount,
            RepeatEvery: formValues.repeatEvery,
            RepeatOnList: chosenDays
        }
        this.locationService.getLocation(formValues.address).subscribe((res: Location) => {
            newEvent.LocationId = res.Id;
            this.loaderService.displayLoader(false);
            this.eventEmitter.emit(newEvent);
        })
    }

    clearLocation(): void {
        this.isAddressValid = false;
        this.eventForm.controls["address"].setValue("");
        this.eventForm.controls["latitude"].setValue(undefined);
        this.eventForm.controls["longitude"].setValue(undefined);
    }

    // when user writes number in inputs in "end of repeating"
    endOfRepeatingBlured(valueType: string, value: string) {
        this.repeat.value = value;
        this.repeat.valueType = valueType;
    }

    endOfRepeatingNumber(): number {
        if (this.repeat.valueType == "num") {
            return +this.repeat.value;
        } else if (this.repeat.valueType == "date") {
            let endDateTimeRecurring: Date = new Date(this.repeat.value);
            let endDateTime: Date = new Date(this.end.value);
            let lastDateTime: Date = endDateTime;
            let repeating = parseInt(this.repeatEvery.value);
            let numberOfRepeating: number = 0;

            if (this.occurrence.value == "daily") {
                lastDateTime.setDate(lastDateTime.getDate() + repeating);

                while (lastDateTime < endDateTimeRecurring) {
                    lastDateTime.setDate(lastDateTime.getDate() + repeating);
                    numberOfRepeating += 1;
                }
            } else if (this.occurrence.value == "weekly") {
                repeating *= 7;
                lastDateTime.setDate(lastDateTime.getDate() + repeating);

                while (lastDateTime < endDateTimeRecurring) {
                    lastDateTime.setDate(lastDateTime.getDate() + repeating);
                    numberOfRepeating += 1;
                }
            } else if (this.occurrence.value == "monthly" && this.monthlyOption == "month") {
                let day = lastDateTime.getDate();
                lastDateTime.setMonth(lastDateTime.getMonth() + repeating);
                lastDateTime.setDate(day);

                while (lastDateTime.getDate() != day) {
                    lastDateTime.setMonth(lastDateTime.getMonth() + repeating);
                    lastDateTime.setDate(day);
                }

                while (lastDateTime < endDateTimeRecurring) {
                    lastDateTime.setMonth(lastDateTime.getMonth() + repeating);
                    lastDateTime.setDate(day);

                    while (lastDateTime.getDate() != day) {
                        lastDateTime.setMonth(lastDateTime.getMonth() + repeating);
                        lastDateTime.setDate(day);
                    }
                    numberOfRepeating += 1;
                }
            } else if (this.occurrence.value == "monthly" && this.monthlyOption == "week") {
                let currentWeek = getWeekOfMonth(lastDateTime);
                let currentDay = lastDateTime.getDay();
                let nextMonth;

                nextMonth = (lastDateTime.getMonth() + repeating) % 12;
                lastDateTime = new Date(lastDateTime.getFullYear(), lastDateTime.getMonth() + repeating, 1);
                lastDateTime.setDate(1 + (7 + currentDay - lastDateTime.getDay()) % 7);
                lastDateTime = setWeekOfMonth(lastDateTime, currentWeek);

                while (lastDateTime.getMonth() != nextMonth && lastDateTime < endDateTimeRecurring) { // tjedan s određenim danom u određenom mjesecu ne postoji
                    lastDateTime = new Date(lastDateTime.getFullYear(), nextMonth + repeating, 1);
                    nextMonth = (nextMonth + repeating) % 12;
                    lastDateTime.setDate(1 + (7 + currentDay - lastDateTime.getDay()) % 7);
                    lastDateTime = setWeekOfMonth(lastDateTime, currentWeek);
                }

                while (lastDateTime < endDateTimeRecurring) {
                    nextMonth = (lastDateTime.getMonth() + repeating) % 12;
                    lastDateTime = new Date(lastDateTime.getFullYear(), lastDateTime.getMonth() + repeating, 1);
                    lastDateTime.setDate(1 + (7 + currentDay - lastDateTime.getDay()) % 7);
                    lastDateTime = setWeekOfMonth(lastDateTime, currentWeek);

                    while (lastDateTime.getMonth() != nextMonth && lastDateTime < endDateTimeRecurring) { // tjedan s određenim danom u određenom mjesecu ne postoji
                        lastDateTime = new Date(lastDateTime.getFullYear(), nextMonth + repeating, 1);
                        nextMonth = (nextMonth + repeating) % 12;
                        lastDateTime.setDate(1 + (7 + currentDay - lastDateTime.getDay()) % 7);
                        lastDateTime = setWeekOfMonth(lastDateTime, currentWeek);
                    }
                    numberOfRepeating += 1;
                }
            } else if (this.occurrence.value == "yearly") {
                let feb29 = lastDateTime.getDate() == 29 && lastDateTime.getMonth() == 1;

                if (feb29) {
                    lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);

                    while (!isLeapYear(lastDateTime.getFullYear()) && lastDateTime < endDateTimeRecurring) {
                        lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);
                    }

                    lastDateTime.setMonth(1);
                    lastDateTime.setDate(29);
                } else {
                    lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);
                }

                while (lastDateTime < endDateTimeRecurring) {
                    if (feb29) {
                        lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);

                        while (!isLeapYear(lastDateTime.getFullYear()) && lastDateTime < endDateTimeRecurring) {
                            lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);
                        }

                        lastDateTime.setMonth(1);
                        lastDateTime.setDate(29);
                    } else {
                        lastDateTime.setFullYear(lastDateTime.getFullYear() + repeating);
                    }

                    numberOfRepeating += 1;
                }
            }

            console.log(numberOfRepeating);
            return numberOfRepeating;
        }
    }

    // disabling and enabling inputs in "end of repeating" part and deleting their contents
    endOfRepeatingChanged(containerOfElements: any[]) {
        for (let con of containerOfElements) {
            if (con.disable) {
                con.element.disabled = "disabled";
                con.element.value = "";
            } else {
                con.element.disabled = "";
            }
        }
    }

    isAllUnchecked(): boolean {
        let checkbox: ICategoryElement
        for (checkbox of this.categoryService.categories) {
            if (checkbox.checked) {
                return false;
            }
        }
        return true;
    }

    markerUpdated(event: any): void {
        this.eventForm.controls["latitude"].setValue( event.coords.lat);
        this.eventForm.controls["longitude"].setValue(event.coords.lng);
        this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
            this.eventForm.controls["address"].setValue(response);
        });
    }

    monthlyOptionChange() {
        switch (this.monthlyOption) {
            case "month":
                this.monthlyOption = "week";
                break;
            case "week":
                this.monthlyOption = "month";
                break;
        }
    }

    // returns array of numbers: [1..n]
    range(n: number): number[] {
        return Array.from(Array(n + 1).keys()).slice(1);
    }

    updateCategories(category: number): void {
        this.categoryService.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

    updateDays(dayNumber: number): void {
        this.categoryService.days.filter(checkbox => {
            if (checkbox.id == dayNumber) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

    private setCurrentPosition(): void {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.eventForm.controls["latitude"].setValue(position.coords.latitude);
                this.eventForm.controls["longitude"].setValue(position.coords.longitude);
                this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
                    this.eventForm.controls["address"].setValue(response);
                    if (this.eventForm.value.address !== "") {
                        this.isAddressValid = true;
                        this.eventForm.controls["address"].markAsTouched();
                    }
                });
            });
        }
    }
}

interface ICategoryElement {
    id: number,
    checked: boolean
}

let getWeekOfMonth = function (date: Date, exact: boolean = true) {
    var month = date.getMonth()
        , year = date.getFullYear()
        , firstWeekday = new Date(year, month, 1).getDay()
        , lastDateOfMonth = new Date(year, month + 1, 0).getDate()
        , offsetDate = date.getDate() + firstWeekday - 1
        , index = 1 // start index at 0 or 1, your choice
        , weeksInMonth = index + Math.ceil((lastDateOfMonth + firstWeekday - 7) / 7)
        , week = index + Math.floor(offsetDate / 7)
        ;
    if (exact || week < 2 + index) return week;
    return week === weeksInMonth ? index + 5 : week;
};

let setWeekOfMonth = function (date: Date, week: number) {
    date.setDate(date.getDate() + (week - getWeekOfMonth(date))*7);
	return date;
}

function isLeapYear(year: number) {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}