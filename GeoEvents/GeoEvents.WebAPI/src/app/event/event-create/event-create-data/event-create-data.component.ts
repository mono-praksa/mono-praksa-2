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
    price: FormControl;
    start: FormControl;

    private createdEvent: Event;
    private createEventLoading: boolean = false;
    private isAddressValid: boolean = false;
    private zoom: number = 12;

    constructor(
        private categoryService: CategoryService,
        private eventService: EventService,
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
        this.name = new FormControl("", Validators.required);
        this.description = new FormControl("", Validators.required);
        this.start = new FormControl("", Validators.required);
        this.end = new FormControl("", Validators.required);
        this.price = new FormControl("", [Validators.required, Validators.pattern(/^[0-9]+(\.\d{1,2})?$/)]);
        this.capacity = new FormControl("", [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]);
        this.address = new FormControl("", Validators.required);
        this.latitude = new FormControl("", Validators.required);
        this.longitude = new FormControl("", Validators.required);

        this.eventForm = new FormGroup({
            name: this.name,
            description: this.description,
            start: this.start,
            end: this.end,
            price: this.price,
            capacity: this.capacity,
            address: this.address,
            latitude: this.latitude,
            longitude: this.longitude
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
            Id: undefined,
            Name: formValues.name,
            Description: formValues.description,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Latitude: formValues.latitude,
            Longitude: formValues.longitude,
            Categories: chosenCategories,
            Price: formValues.price,
            Capacity: formValues.capacity,
            Reserved: undefined,
            Rating: undefined,
            RateCount: undefined,
            CustomModel: undefined,
            Custom: undefined,
            LocationId: undefined
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