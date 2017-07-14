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
    lat: number;
    lng: number;

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
	searchString: FormControl
	searchNameOnly: FormControl
	
    latitude: number
    longitude: number

    mapMode: boolean = false
    detailsMode: boolean = false

    events: IEvent[]
    event: IEvent
	
	oldFilter: IFilter

    constructor(private http: Http, private formBuilder: FormBuilder, private mapsAPILoader: MapsAPILoader, private ngZone: NgZone) { }

    ngOnInit() {
        this.start = new FormControl('');
        this.end = new FormControl('');
        this.address = new FormControl('');
        this.radius = new FormControl('');
		this.searchString = new FormControl('');
		this.searchNameOnly = new FormControl('');

		//build the form
        this.filterForm = this.formBuilder.group({
            start: this.start,
            end: this.end,
            address: this.address,
            radius: this.radius,
			searchString: this.searchString,
			searchNameOnly: this.searchNameOnly
        });

		//set current location
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
                this.lat = position.coords.latitude;
                this.lng = position.coords.longitude;
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

	//this function is called when the submit button is pressed
	//it sets undefined filter values to some default values and saves the filter into the variable
	//then it calls the getEvents function which gets the events form the server
    onSubmit(formValues: any) {	
		//set the chosen categories 
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
		
		//set lat and long (for map component, so the location is reset on change)
        this.lat = this.latitude
        this.lng = this.longitude

		//get values from the forms and set the default values
        var filter: IFilter = {
            ULat: this.latitude,
            ULong: this.longitude,
            Radius: formValues.radius,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Category: cat,
			SearchString: formValues.searchString,
			SearchNameOnly: formValues.searchNameOnly,
			
			PageNumber: 1,
			PageSize: 10,
			OrderByString: "Name",
			OrderIsAscending: true
        }

		//get the events
        this.getEvents(filter).subscribe(res => {
            this.events = res
            this.latitude = this.lat
            this.longitude = this.lng
        })
    }
	
	getEventsAscendingChanged(isAscending: boolean){
		let filter = this.oldFilter;
		filter.OrderIsAscending = !filter.OrderIsAscending;
		//get the events
        this.getEvents(filter).subscribe(res => {
            this.events = res
            this.latitude = this.lat
            this.longitude = this.lng
        })
	}
	
	getEventsOrderChanged(newOrder: string){
		let filter = this.oldFilter;
		filter.OrderByString = newOrder
		this.getEvents(filter).subscribe(res => {
            this.events = res
            this.latitude = this.lat
            this.longitude = this.lng
        })
	}
	//this function is called when a new page is selected or when the order parameters changed
	getEventsNextPage()
	{
		let filter = this.oldFilter;
		filter.PageNumber = filter.PageNumber + 1;
		
		//get the events
        this.getEvents(filter).subscribe(res => {
            this.events = res
            this.latitude = this.lat
            this.longitude = this.lng
        })
	}
	
	getEventsPreviousPage()
	{
		let filter = this.oldFilter;
		if(filter.PageNumber > 1){
			filter.PageNumber = filter.PageNumber - 1;
			
			//get the events
			this.getEvents(filter).subscribe(res => {
				this.events = res
				this.latitude = this.lat
				this.longitude = this.lng
			})
		}
	}
	
	getEventsFirstPage()
	{
		let filter = this.oldFilter;
		filter.PageNumber = 1;
		
		//get the events
        this.getEvents(filter).subscribe(res => {
            this.events = res
            this.latitude = this.lat
            this.longitude = this.lng
        })
	}

	//this builds the querry from the filter and gets the events
    getEvents(filter: IFilter): Observable<IEvent[]> {
		//save the filter to preserve it for use in other function
		this.oldFilter = filter;
		
		//building the querry 
		let querry = '/api/events/search?category=';
		querry += filter.Category.toString();
		
		if(filter.ULat != null && filter.ULong != null && filter.Radius != null && filter.Radius != 0)
		{
			querry += '&uLat=' + filter.ULat.toString();
			querry += '&uLong=' + filter.ULong.toString(); 
			querry += '&radius=' + filter.Radius.toString();
		}
		
		if(filter.StartTime != null)
		{
			querry += '&startTime=' + filter.StartTime.toString().replace(':', 'h');
		}
		
		if(filter.EndTime != null)
		{
			querry += '&endTime=' + filter.EndTime.toString().replace(':', 'h');
		}
		
		if(filter.SearchString != null)
		{
			querry += '&searchString=' + filter.SearchString.toString();
		}
		
		if(filter.SearchNameOnly != null)
		{
			querry += '&nameOnly=' + filter.SearchNameOnly.toString();
		}

		querry += '&pageNumber=' + filter.PageNumber.toString();
		querry += '&pageSize=' + filter.PageSize.toString();
		querry += '&orderAscending=' + filter.OrderIsAscending.toString();
		querry += '&orderBy=' + filter.OrderByString.toString();
		
		//execute http call
        return this.http.get(querry).map(function (response: Response) {
            return <IEvent[]>response.json();
        }).catch(this.handleError);
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }
}