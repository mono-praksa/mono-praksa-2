import { MapsAPILoader } from "@agm/core";
import { Component, ElementRef, EventEmitter, NgZone, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { Observable } from "rxjs/Observable";

import { CategoryService } from "../../shared/category.service";
import { EventService } from "../../shared/event.service";
import { endDateBeforeStartDate, uniqueName, startDayNotCheckedIfWeekly } from "../../shared/validator";
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
    repeatEvery: FormControl;
    repeatOnList: FormControl;
    start: FormControl;

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

	//called on init. starts the loader service, builds the form, starts the adresss atocomplete
	//starts the maps api loader
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

	//creates the form
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
        this.repeatOnList = new FormControl([]);

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
            repeatEvery: this.repeatEvery,
            repeatOnList: this.repeatOnList
        });
        this.eventForm.setValidators([endDateBeforeStartDate("start", "end"), startDayNotCheckedIfWeekly()]);
    }

	//gets the form values and calls the service to create the event 
    createEvent(formValues: any) {
        this.loaderService.displayLoader(true);
        let chosenCategories: number[] = [];
        let repeatCount: number = 0;

        this.categoryService.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

        if (this.occurrence.value != "none") {
            repeatCount = this.endOfRepeatingNumber();
        }

        let occurrence = formValues.occurrence;

        if (this.monthlyOption == "week" && this.occurrence.value == 'monthly') {
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
            RepeatOnList: formValues.repeatOnList
        }
        
        this.locationService.getLocation(formValues.address).subscribe((res: Location) => {
            newEvent.LocationId = res.Id;
            this.loaderService.displayLoader(false);
            this.eventEmitter.emit(newEvent);
        })
    }

    checkDay(day: number) {
        for (let el of this.categoryService.days) {
            if (el.id == day) {
                return el.checked;
            }
        }

        this.updateDays(day);
    }

    checkDayDefault() {
        if (this.occurrence.value == 'weekly') {
            if (this.start.value != "") {
                this.updateDays(Math.pow(2, new Date(this.start.value).getDay()));
            }
        } else {
            this.categoryService.buildCategories("day");
            this.repeatOnList.setValue([]);
        }
    }

	//clears the location from the form control
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

	//counts the number of times the event will reocurr
    endOfRepeatingNumber(): number {
        if (this.repeat.valueType == "num") {
            return +this.repeat.value;
        } else if (this.repeat.valueType == "date") {
            let endDateTimeRecurring: Date = new Date(this.repeat.value);
            let endDateTime: Date = new Date(this.end.value);
            let lastDateTime: Date = endDateTime;
            let repeating = parseInt(this.repeatEvery.value);
            let numberOfRepeating: number = 0;

            let dayOccurrence = getDayOccurrenceInMonth(new Date(lastDateTime)); // used if monthly by day name

            if (this.occurrence.value == "daily") {
                do {
                    lastDateTime.setDate(lastDateTime.getDate() + repeating);
                    numberOfRepeating += 1;
                } while (lastDateTime < endDateTimeRecurring);
            } else if (this.occurrence.value == "weekly") {
                let listOfDays = this.categoryService.days.map((n: { id: number, checked: boolean }) => { if (n.checked) return Math.log2(n.id);});
                listOfDays.push(Infinity);
                listOfDays.sort();
                
                let moveDay;

                do {
                    for (let i = 0; i < listOfDays.length; i++) {
                        if (listOfDays[i] >= lastDateTime.getDay()) {
                            if (listOfDays[i] > lastDateTime.getDay()) {
                                moveDay = listOfDays[i];
                            } else {
                                moveDay = listOfDays[i + 1];
                            }
                            break;
                        }
                    }

                    if (moveDay == Infinity) {
                        let value: number = repeating * 7 + listOfDays[0] - lastDateTime.getDay();

                        lastDateTime.setDate(lastDateTime.getDate() + value);
                    } else {
                        lastDateTime.setDate(lastDateTime.getDate() + moveDay - lastDateTime.getDay());
                    }

                    numberOfRepeating += 1;
                } while (lastDateTime < endDateTimeRecurring);
            } else if (this.occurrence.value == "monthly" && this.monthlyOption == "month") {
                let day = lastDateTime.getDate();

                let nextTime;
                let nextTimeNumberOfDays;

                do {
                    nextTime = new Date(lastDateTime.getFullYear(), lastDateTime.getMonth() + repeating, 1);
                    nextTimeNumberOfDays = new Date(nextTime.getFullYear(), nextTime.getMonth() + 1, 0).getDate();

                    while (day > nextTimeNumberOfDays) {
                        nextTime = new Date(nextTime.getFullYear(), nextTime.getMonth() + repeating, 1);
                        nextTimeNumberOfDays = new Date(nextTime.getFullYear(), nextTime.getMonth() + 1, 0).getDate();
                    }

                    nextTime.setDate(day);
                    nextTime.setHours(lastDateTime.getHours());
                    nextTime.setMinutes(lastDateTime.getMinutes());

                    lastDateTime = nextTime;

                    numberOfRepeating += 1;
                } while (lastDateTime < endDateTimeRecurring);
            } else if (this.occurrence.value == "monthly" && this.monthlyOption == "week") {
                let nextTime = new Date(lastDateTime.getFullYear(), lastDateTime.getMonth() + repeating, 1);

                let nextTimeDay;
                let currentDay;

                do {
                    nextTime = new Date(lastDateTime.getFullYear(), lastDateTime.getMonth() + repeating, 1);

                    nextTimeDay = nextTime.getDay();
                    currentDay = lastDateTime.getDay();

                    if (currentDay >= nextTimeDay) {
                        nextTime.setDate(nextTime.getDate() + currentDay - nextTimeDay);
                    } else {
                        nextTime.setDate(nextTime.getDate() + 7 - (nextTimeDay - currentDay));
                    }

                    nextTime = setDayOccurrenceInMonth(nextTime, dayOccurrence);
                    nextTime.setHours(lastDateTime.getHours());
                    nextTime.setMinutes(lastDateTime.getMinutes());

                    lastDateTime = nextTime;

                    numberOfRepeating += 1;
                } while (lastDateTime < endDateTimeRecurring);
            } else if (this.occurrence.value == "yearly") {
                let nextTime;

                do {
                    nextTime = new Date(lastDateTime.getFullYear() + repeating, lastDateTime.getMonth(), 1);

                    if (lastDateTime.getDate() == 29 && lastDateTime.getMonth() == 1) { // if feb29
                        while (lastDateTime.getDate() == 29 && lastDateTime.getMonth() == 1 && !isLeapYear(nextTime.getFullYear())) {
                            nextTime = new Date(nextTime.getFullYear() + repeating, nextTime.getMonth(), 1);
                        }
                    }

                    nextTime.setDate(lastDateTime.getDate());
                    nextTime.setHours(lastDateTime.getHours());
                    nextTime.setMinutes(lastDateTime.getMinutes());
                    lastDateTime = nextTime;

                    numberOfRepeating += 1;
                } while (lastDateTime < endDateTimeRecurring);
            }

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

	//checks wether all category checkboxes are unchecked
    isAllUnchecked(): boolean {
        for (let checkbox of this.categoryService.categories) {
            if (checkbox.checked) {
                return false;
            }
        }
        return true;
    }

	//called when the google maps marker is updateDays
	//gets the adress from the latitude and loingtude and sets the form controll
    markerUpdated(event: any): void {
        this.eventForm.controls["latitude"].setValue( event.coords.lat);
        this.eventForm.controls["longitude"].setValue(event.coords.lng);
        this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
            this.eventForm.controls["address"].setValue(response);
        });
    }

	//called on changing the option for reoccuring events
	//sets the option 
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

	//gets the selected categories and updates the checkboxes
    updateCategories(category: number): void {
        this.categoryService.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

	//update days for recurrring events
    updateDays(dayNumber: number): void {
        let chosenDays: number[] = [];

        this.categoryService.days.filter(checkbox => {
            if (checkbox.id == dayNumber) {
                checkbox.checked = !checkbox.checked;
            }
        });

        for (let el of this.categoryService.days) {
            if (el.checked) {
                chosenDays.push(el.id);
            }
        }
        
        this.repeatOnList.setValue(chosenDays);
    }

	//sets the current position using geolocation services
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

let getDayOccurrenceInMonth = function (date: Date): number {
    let day = date.getDate();

    if (day <= 7) {
        return 1;
    } else if (day <= 14) {
        return 2;
    } else if (day <= 21) {
        return 3;
    } else if (day <= 28) {
        return 4;
    } else if (day <= 31) {
        return 5;
    }
}

let isLeapYear = function (year: number): boolean {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}

let setDayOccurrenceInMonth = function (date: Date, occurrence: number): Date {
    let currentDay = date.getDay();

    let newDate = new Date(date.getFullYear(), date.getMonth(), (occurrence - 1) * 7 + 1);
    let dayOfNewDate = newDate.getDay();

    if (currentDay >= dayOfNewDate) {
        newDate.setDate(newDate.getDate() + currentDay - dayOfNewDate);
    } else {
        newDate.setDate(newDate.getDate() + 7 - (dayOfNewDate - currentDay));
    }

    if (newDate.getMonth() != date.getMonth()) {
        return setDayOccurrenceInMonth(date, 4);
    }

    return newDate;
}