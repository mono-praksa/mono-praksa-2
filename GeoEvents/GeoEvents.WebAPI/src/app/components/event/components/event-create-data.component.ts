import { Component, OnInit, ElementRef, NgZone, ViewChild, Output, EventEmitter } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Observable } from 'rxjs/Rx'
import { MapsAPILoader } from '@agm/core'

import { endDateBeforeStartDate } from '../validators/validator'
import { IEvent } from '../models/event.model'
import { CategoryEnum } from '../../../shared/common/category-enum'
import { LoaderService } from '../../../shared/loader.service'
import { EventService } from '../event.service'

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
    private _createEventLoading: boolean = false;
    private _createdEvent: IEvent;

    //variables for google maps api
    private _latitude: number = 0;
    private _longitude: number = 0;
    private _zoom: number = 12;

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

    constructor(
        private _mapsAPILoader: MapsAPILoader,
        private _ngZone: NgZone,
        private _loaderService: LoaderService,
        private _eventService: EventService
    ) { }

    ngOnInit(): void {
        this.buildForm();

        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.createEventLoading = value;
        });

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
                    this.latitude = place.geometry.location.lat();
                    this.longitude = place.geometry.location.lng();
                });
            });
        });
    }

    private buildForm(): void {
        this.name = new FormControl('', Validators.required);
        this.description = new FormControl('', Validators.required);
        this.start = new FormControl('', Validators.required);
        this.end = new FormControl('', Validators.required);
        this.price = new FormControl('', Validators.required);
        this.capacity = new FormControl('', Validators.required);

        this.eventForm = new FormGroup({
            name: this.name,
            description: this.description,
            start: this.start,
            end: this.end,
            price: this.price,
            capacity: this.capacity
        }, (formGroup: FormGroup) => {
            return endDateBeforeStartDate(formGroup);
        });
    }

    private setCurrentPosition(): void {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
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
            Lat: this.latitude,
            Long: this.longitude,
            Categories: chosenCategories,
            Price: formValues.price,
            Capacity: formValues.capacity,
            Reserved: 0,
            Rating: 0,
            RateCount: 0
        }
        console.log(newEvent);

        this._eventService.createEvent(newEvent).subscribe((response: IEvent) => {
            this.createdEvent = response;
            this.eventEmitter.emit(this.createdEvent);
            this._loaderService.displayLoader(false);
        });
    }

    updateCategories(category: number): void {
        this.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

    mapClicked(event: any): void {
        this.latitude = event.coords.lat;
        this.longitude = event.coords.lng;
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

    get latitude(): number {
        return this._latitude;
    }

    set latitude(theLatitude: number) {
        this._latitude = theLatitude;
    }

    get longitude(): number {
        return this._longitude;
    }

    set longitude(theLongitude: number) {
        this._longitude = theLongitude;
    }

    get zoom(): number {
        return this._zoom;
    }

    set zoom(theZoom: number) {
        this._zoom = theZoom;
    }

    get createEventLoading(): boolean {
        return this._createEventLoading;
    }

    set createEventLoading(isCreatingEvent: boolean) {
        this._createEventLoading = isCreatingEvent;
    }

    get createdEvent(): IEvent {
        return this._createdEvent;
    }

    set createdEvent(theCreatedEvent: IEvent) {
        this._createdEvent = theCreatedEvent;
    }
}

interface ICategoryElement {
    id: number,
    checked: boolean
}