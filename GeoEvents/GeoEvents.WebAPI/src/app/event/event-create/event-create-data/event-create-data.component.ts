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
    recurring: string = "none";
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
    occurence: FormControl;
    price: FormControl;
    start: FormControl;

    // FormControls for recurring
    repeatCount: FormControl;
    repeatEvery: FormControl;
    repeatOn: FormControl;

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

        this.categoryService.buildCategories(true);

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
        this.occurence = new FormControl("none");
        this.price = new FormControl("", [Validators.required, Validators.pattern(/^[0-9]+(\.\d{1,2})?$/)]);
        this.start = new FormControl("", Validators.required);

        // FormControls for reccuring
        this.repeatCount = new FormControl(0);
        this.repeatEvery = new FormControl(1);
        this.repeatOn = new FormControl("");

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
            occurence: this.occurence,

            // FormControls for reccuring
            repeatCount: this.repeatCount,
            repeatEvery: this.repeatEvery,
            repeatOn: this.repeatOn
        }, endDateBeforeStartDate("start", "end"));
    }

    createEvent(formValues: any) {
        this.loaderService.displayLoader(true);
        let chosenCategories: number[] = [];
        this.categoryService.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

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
            Occurence: formValues.occurence,
            Price: formValues.price,
            RateCount: undefined,
            Rating: undefined,
            Reserved: undefined,
            StartTime: formValues.start,

            // attributes for reccuring events
            RepeatCount: formValues.repeatCount,
            RepeatEvery: formValues.repeatEvery,
            RepeatOn: formValues.repeatOn
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
    endOfRepeatingBlured(value: number = undefined) {
        if (value) {
            this.repeatCount.setValue(+value);
        } else {
            this.repeatCount.setValue(0);
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

    recurringChanged(value: string) {
        this.recurring = value;

        if (value == 'daily') {

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