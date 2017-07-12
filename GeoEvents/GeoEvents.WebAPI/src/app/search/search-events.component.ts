import { Component, OnInit, ElementRef, NgZone, ViewChild } from '@angular/core'
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'
import { MapsAPILoader } from '@agm/core'

import { IEvent } from './../models/event.model'
import { IFilter } from './../models/filter.model'

@Component({
    templateUrl: "app/search/search-events.component.html",
    styles: [`/* The switch - the box around the slider */
.switch {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 34px;
}

/* Hide default HTML checkbox */
.switch input {display:none;}

/* The slider */
.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: .4s;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 26px;
  width: 26px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  -webkit-transition: .4s;
  transition: .4s;
}

input:checked + .slider {
  background-color: #2196F3;
}

input:focus + .slider {
  box-shadow: 0 0 1px #2196F3;
}

input:checked + .slider:before {
  -webkit-transform: translateX(26px);
  -ms-transform: translateX(26px);
  transform: translateX(26px);
}`]

})
export class SearchEventsComponent implements OnInit {

    @ViewChild("search")
    searchElementRef: ElementRef;

    categories: any[] = [
        { id: 1, checked: false },
        { id: 2, checked: false },
        { id: 4, checked: false },
        { id: 8, checked: false },
        { id: 16, checked: false },
        { id: 32, checked: false },
        { id: 64, checked: false }
    ]
    filterForm: FormGroup

    start: FormControl
    end: FormControl
    category: FormControl
    address: FormControl
    radius: FormControl
    latitude: number
    longitude: number

    mapMode: boolean = false
    detailsMode: boolean = false

    events: IEvent[]
    event: IEvent

    constructor(private http: Http, private formBuilder: FormBuilder, private mapsAPILoader: MapsAPILoader, private ngZone: NgZone) { }

    ngOnInit() {
        this.start = new FormControl('');
        this.end = new FormControl('');
        this.address = new FormControl('');
        this.radius = new FormControl('');

        this.filterForm = this.formBuilder.group({
            start: this.start,
            end: this.end,
            address: this.address,
            radius: this.radius
        });

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
                });
            });
        });
    }

    private setCurrentPosition() {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
            });
        }
    }


    updateCategories(category: number) {
        this.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }



    changeDisplayMode() {
        this.mapMode = !this.mapMode;
    }

    eventDetails(event: IEvent) {
        this.event = event
        this.detailsMode = true
    }

    filterEvents(formValues: any) {
        let chosenCategories: number[] = [];
        this.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });

        var cat = 0
        for (let c of chosenCategories) {
            cat += c
        }
        var filter : IFilter = {
            ULat: this.latitude,
            ULong: this.longitude,
            Radius: formValues.radius,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Category: cat
        }

        console.log('form values: ', formValues)
        console.log('filter: ', filter)
        this.getEvents(filter).subscribe(res => {
            this.events = res
        })
    }

    getEvents(filter: IFilter): Observable<IEvent[]> {

        return this.http.get('/api/event/search/' + filter.ULat.toString() + '/' + filter.ULong.toString() + '/' + filter.Radius.toString() + '/' + filter.Category.toString() + '/' + filter.StartTime.toString().replace(':', 'h') + '/' + filter.EndTime.toString().replace(':', 'h')).map(function (response: Response) {
            return <IEvent[]>response.json();
        }).catch(this.handleError);
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }
}