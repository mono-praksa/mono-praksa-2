import { Component, OnInit, ElementRef, NgZone, ViewChild, Output, EventEmitter } from '@angular/core'
import { FormControl, FormGroup, Validators, ValidatorFn } from '@angular/forms'
import { Observable } from 'rxjs/Rx'
import { MapsAPILoader } from '@agm/core'

import { endDateBeforeStartDate, uniqueName } from '../validators/validator';
import { IEvent } from '../models/event.model';
import { ILocation } from '../models/location.model';
import { CategoryEnum } from '../../../shared/common/category-enum';
import { LoaderService } from '../../../shared/loader.service';
import { EventService } from '../event.service';
import { LocationService } from '../location.service';
import { GeocodingService } from '../../../shared/geocoding.service';

@Component({
    selector: "create-event",
    templateUrl: "app/components/event/views/event-create-data.component.html",
    styles: [`
        agm-map {
            height: 300px;
        }
    `]
})
export class EventCreateDataComponent implements OnInit {
    @Output() eventEmitter = new EventEmitter();
    @ViewChild("search") searchElementRef: ElementRef;
    private _createdEvent: IEvent;
    private _createEventLoading: boolean = false;

    //variables for google maps api
    private _zoom: number = 12;
    private _isAddressValid: boolean = false;

    CategoryEnum: any = CategoryEnum;

    categories: Array<ICategoryElement> = [
        { id: CategoryEnum["Music"], checked: false },
        { id: CategoryEnum["Culture"], checked: false },
        { id: CategoryEnum["Sport"], checked: false },
        { id: CategoryEnum["Gastro"], checked: false },
        { id: CategoryEnum["Religious"], checked: false },
        { id: CategoryEnum["Business"], checked: false },
        { id: CategoryEnum["Miscellaneous"], checked: true }
    ]

    eventForm: FormGroup;
    name: FormControl;
    description: FormControl;
    start: FormControl;
    end: FormControl;
    category: FormControl;
    price: FormControl;
    capacity: FormControl;
    address: FormControl;
    latitude: FormControl;
    longitude: FormControl;

    constructor(
        private _mapsAPILoader: MapsAPILoader,
        private _ngZone: NgZone,
        private _loaderService: LoaderService,
        private _eventService: EventService,
        private _locationService: LocationService,
        private geocodingService: GeocodingService
    ) { }

    ngOnInit(): void {
        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.createEventLoading = value;
        });

        this.buildForm();

        //GOOGLE MAPS
        this.setCurrentPosition();

        this._mapsAPILoader.load().then(() => {
            let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
                types: ["address"]
            });
            autocomplete.addListener("place_changed", () => {
                this._ngZone.run(() => {
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

    clearLocation(): void {
        this.isAddressValid = false;
        this.eventForm.controls["address"].setValue("");
        this.eventForm.controls["latitude"].setValue(null);
        this.eventForm.controls["longitude"].setValue(null);
    }

    private buildForm(): void {
        this.name = new FormControl('', Validators.required);
        this.description = new FormControl('', Validators.required);
        this.start = new FormControl('', Validators.required);
        this.end = new FormControl('', Validators.required);
        this.price = new FormControl('', [Validators.required, Validators.pattern(/^[0-9]+(\.\d{1,2})?$/)]);
        this.capacity = new FormControl('', [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]);
        this.address = new FormControl('', Validators.required);
        this.latitude = new FormControl('', Validators.required);
        this.longitude = new FormControl('', Validators.required);

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
        }, endDateBeforeStartDate('start', 'end'));
    }

    private setCurrentPosition(): void {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.eventForm.controls["latitude"].setValue(position.coords.latitude);
                this.eventForm.controls["longitude"].setValue(position.coords.longitude);
                this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
                    this.eventForm.controls["address"].setValue(response);
                });
                this.isAddressValid = true;
            });
        }
    }

    createEvent(formValues: any) {
        this._loaderService.displayLoader(true);
        let chosenCategories: number[] = [];
        this.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

        let newEvent : IEvent = {
            Id: undefined,
            Name: formValues.name,
            Description: formValues.description,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Lat: formValues.latitude,
            Long: formValues.longitude,
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
        this._locationService.getLocation(formValues.address).subscribe((res: ILocation) => {
            newEvent.LocationId = res.Id;
            this._loaderService.displayLoader(false);
            console.log(newEvent);
            this.eventEmitter.emit(newEvent);
        })
    }

    updateCategories(category: number): void {
        this.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

    markerUpdated(event: any): void {
        this.eventForm.controls["latitude"].setValue( event.coords.lat);
        this.eventForm.controls["longitude"].setValue(event.coords.lng);
        this.geocodingService.getAddress(this.eventForm.controls["latitude"].value, this.eventForm.controls["longitude"].value).subscribe(response => {
            this.eventForm.controls["address"].setValue(response);
        });
    }

    isAllUnchecked(): boolean {
        let checkbox: ICategoryElement
        for (checkbox of this.categories) {
            if (checkbox.checked) {
                return false;
            }
        }
        return true;
    }

    keys(): Array<string> {
        var keys = Object.keys(CategoryEnum);
        return keys.slice(keys.length / 2);
    }

    get zoom(): number {
        return this._zoom;
    }

    set zoom(theZoom: number) {
        this._zoom = theZoom;
    }

    get createdEvent(): IEvent {
        return this._createdEvent;
    }

    set createdEvent(theCreatedEvent: IEvent) {
        this._createdEvent = theCreatedEvent;
    }

    get isAddressValid(): boolean {
        return this._isAddressValid;
    }

    set isAddressValid(isAddressValid: boolean) {
        this._isAddressValid = isAddressValid;
    }

    get createEventLoading(): boolean {
        return this._createEventLoading;
    }

    set createEventLoading(isCreatingEvent: boolean) {
        this._createEventLoading = isCreatingEvent;
    }  
}

interface ICategoryElement {
    id: number,
    checked: boolean
}