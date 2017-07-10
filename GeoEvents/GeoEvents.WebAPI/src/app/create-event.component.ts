import { Component, OnInit, ElementRef, NgZone, ViewChild } from '@angular/core'
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'
import { MapsAPILoader } from '@agm/core'

import { endDateBeforeStartDate } from './validators'

@Component({
    templateUrl: "app/create-event.component.html",
    styles: [`
        agm-map {
            height: 300px;
        }
    `]
})
export class CreateEventComponent implements OnInit {
    latitude: number;
    longitude: number;
    zoom: number;

    @ViewChild("search")
    searchElementRef: ElementRef;

    CategoryEnum: any = CategoryEnum

    categories: any[] = [
        { id: CategoryEnum["Music"], checked: false },
        { id: CategoryEnum["Culture"], checked: false },
        { id: CategoryEnum["Sport"], checked: false },
        { id: CategoryEnum["Gastro"], checked: false },
        { id: CategoryEnum["Religious"], checked: false },
        { id: CategoryEnum["Business"], checked: false },
        { id: CategoryEnum["Miscellaneous"], checked: true }
    ]
    eventForm: FormGroup

    name: FormControl
    description: FormControl
    start: FormControl
    end: FormControl
    category: FormControl

    constructor(private http: Http, private formBuilder: FormBuilder, private mapsAPILoader: MapsAPILoader, private ngZone: NgZone) { }

    ngOnInit() {
        this.name = new FormControl('', Validators.required);
        this.description = new FormControl('', Validators.required);
        this.start = new FormControl('', Validators.required);
        this.end = new FormControl('', Validators.required);

        this.eventForm = this.formBuilder.group({
            name: this.name,
            description: this.description,
            start: this.start,
            end: this.end
        }, { validator: endDateBeforeStartDate('start', 'end') });

        //GOOGLE MAPS
        this.zoom = 4;
        this.latitude = 39.8282;
        this.longitude = -98.5795;

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
                    this.latitude = place.geometry.location.lat();
                    this.longitude = place.geometry.location.lng();
                    this.zoom = 12;
                });
            });
        });
    }

    private setCurrentPosition() {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
                this.zoom = 12;
            });
        }
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }

    createEvent(formValues: any) {
        let chosenCategories: number[] = [];
        this.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

        let newEvent = {
            Name: formValues.name,
            Description: formValues.description,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Lat: this.latitude,
            Long: this.longitude,
            Categories: chosenCategories
        }
        
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post('/api/event/create', JSON.stringify(newEvent), options).map(function (response: Response) {
            return response.json();
        }).catch(this.handleError).subscribe((response: Response) => {
            console.log(response);
        });
    }

    updateCategories(category: number) {
        this.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }

    mapClicked($event: any) {
        this.latitude = $event.coords.lat;
        this.longitude = $event.coords.lng;
    }

    isAllUnchecked() {
        let checkbox: any
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
}

export enum CategoryEnum {
    Music = 1,
    Culture = 2,
    Sport = 4,
    Gastro = 8,
    Religious = 16,
    Business = 32,
    Miscellaneous = 64
}