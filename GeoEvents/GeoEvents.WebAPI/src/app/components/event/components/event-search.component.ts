import { Component, OnInit, DoCheck, ViewChild, NgZone, ElementRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { MapsAPILoader } from '@agm/core';

import { IEvent } from '../models/event.model';
import { IFilter } from '../models/filter.model';

import { PreserveSearchQuerryService } from '../../../shared/preserve-search-querry.service';
import { EventService } from '../event.service';
import { GeocodingService } from '../../../shared/geocoding.service';
import { LoaderService } from '../../../shared/loader.service';

import { needBothOrNeitherOfAddressAndRadius, endDateBeforeStartDate } from '../validators/validator';

@Component({
	templateUrl: 'app/components/event/views/event-search.component.html', 
//	providers: [ PreserveSearchQuerryService ]
})
export class EventSearchComponent implements OnInit {
    private _searchEventLoading: boolean = false;
    private _userApproximateAddress: string = "";

	//variables for storing data
	private _events: IEvent[];
	private _event: IEvent;
	private _errorMessage: string;
	//isLoadingData: boolean = false;
	
	//variables for the filter and retrieving data
    filterForm: FormGroup;
    start: FormControl;
    end: FormControl;
    address: FormControl;
    radius: FormControl;
    searchString: FormControl;
    latitude: FormControl;
    longitude: FormControl;
    price: FormControl;
    rating: FormControl;
	private _filter: IFilter;
	private dataServiceSubscription: Subscription;
	
	//variables for the location services
    isAddressValid: boolean = false;
	
	@ViewChild("search") searchElementRef: ElementRef;
	
	categories: any[] = [
		{ id: 1, checked: false },
		{ id: 2, checked: false },
		{ id: 4, checked: false },
		{ id: 8, checked: false },
		{ id: 16, checked: false },
		{ id: 32, checked: false },
		{ id: 64, checked: false }
	]
	
	//booleans for displaying ui elements
    private _isMapMode: boolean = false;
    private _isDetailMode: boolean = false;
	private _isAdvancedSearch: boolean = false;

    get searchEventLoading(): boolean {
        return this._searchEventLoading;
    }

    set searchEventLoading(thesearchEventLoading: boolean) {
        this._searchEventLoading = thesearchEventLoading;
    }

    get userApproximateAddress(): string {
        return this._userApproximateAddress;
    }

    set userApproximateAddress(theUserApproximateAddress: string) {
        this._userApproximateAddress = theUserApproximateAddress;
    }

    get events(): IEvent[] {
        return this._events;
    }

    set events(theEvents: IEvent[]) {
        this._events = theEvents;
    }

    get event(): IEvent {
        return this._event;
    }	

    set event(theEvent: IEvent) {
        this._event = theEvent;
    }

    get errorMessage(): string {
        return this._errorMessage;
    }

    set errorMessage(theErrorMessage: string) {
        this._errorMessage = theErrorMessage;
    }

    get filter(): IFilter {
        return this._filter;
    }

    set filter(theFilter: IFilter) {
        this._filter = theFilter;
    }

    get isMapMode(): boolean {
        return this._isMapMode;
    }

    set isMapMode(isMapMode: boolean) {
        this._isMapMode = isMapMode;
    }

    get isDetailMode(): boolean {
        return this._isDetailMode;
    }

    set isDetailMode(isDetailMode: boolean) {
        this._isDetailMode = isDetailMode;
    }

    get isAdvancedSearch(): boolean {
        return this._isAdvancedSearch;
    }

    set isAdvancedSearch(isAdvancedSearch: boolean) {
        this._isAdvancedSearch = isAdvancedSearch;
    }

    //constructor
    constructor(
        private _eventService: EventService,
        private _preserveSearchQuerryService: PreserveSearchQuerryService,
        private _mapsAPILoader: MapsAPILoader,
        private _ngZone: NgZone,
        private geocodingService: GeocodingService,
        private _loaderService: LoaderService
    ) {
		this.createForm();
				
		//checking if this service has any params inside, used when redirected from searching in home component
		//if there are params, user likely selected the search button on the home component
		//if there are not any params, user likely clicked on the advanced search button, or redirected from somewhere else.
	}
	
    ngOnInit(): void {
        this.geocodingService.getUserApproximateAddress()
            .subscribe(response => {
                if (response.status == "success") {
                    this.userApproximateAddress = response.city + ", " + response.country;
                }
            });
        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.searchEventLoading = value;
        });

		if(this._preserveSearchQuerryService.searchQuerry != null && this._preserveSearchQuerryService.searchQuerry != ""){
			let newFilter : IFilter = {
				ULat: null,
				ULong: null,
				Radius: 0,
				StartTime: null,
				EndTime: null,
				Category: 127,
				SearchString: this._preserveSearchQuerryService.searchQuerry,
				
				PageNumber: 1,
				PageSize: 10,
				OrderByString: "Name",
				OrderIsAscending: true
			}		
			this.getEvents(newFilter);
		}
		else{
			this.isAdvancedSearch = true;
        }
        this.startMapZoomListener();
	}
	
	//submits
    onSubmit(formValues: any): void {
        this._loaderService.displayLoader(true);
		let selectedCategories = this.getSelectedCategories();
		
		let newFilter : IFilter = {
            ULat: formValues.latitude,
            ULong: formValues.longitude,
            Radius: formValues.radius,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Category: selectedCategories,
			SearchString: formValues.searchString,
			
			PageNumber: 1,
			PageSize: 10,
			OrderByString: "Name",
			OrderIsAscending: true
		}
		
		if (newFilter.SearchString == null) {
			newFilter.SearchString == "";
		}
		
		this.getEvents(newFilter);
	}
	
	//return a number that represents the chosen categories
	//if none are selected it treats it as if all were selected
	getSelectedCategories(): number {
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
		if(cat != 0){
			return cat;
		}
		else {
			return 127;
		}		
	}
	
	//called when the checkbox for one of the categories changes
	//updates the array of categories (i think)
    updateCategories(category: number) {
        this.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }
	
	//sets the position(latitude and longitude) using geolocation services
	private setCurrentPosition(): void {
		//todo: extend this with other location services in case geolocation is blocked by user
		if ("geolocation" in navigator) {
			navigator.geolocation.getCurrentPosition((position) => {
				this.filterForm.controls["latitude"].setValue(position.coords.latitude);
                this.filterForm.controls["longitude"].setValue(position.coords.longitude);
                this.geocodingService.getAddress(this.filterForm.controls["latitude"].value, this.filterForm.controls["longitude"].value).subscribe(response => {
                    this.filterForm.controls["address"].setValue(response);
                });
                this.isAddressValid = true;
			});
		}
	}	
	
	//adds an event listener that listens for changes in the element with the #adress tag
	//once it detects changes and verifies that the adress is correct
	//is sets the latitude and longitude, then Zooms the map on the appropriate position, or so I've been told... 
	private startMapZoomListener() {
		this._mapsAPILoader.load().then(() => {
			let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
				types: ["address"]
			});
			autocomplete.addListener("place_changed", () => {
				this._ngZone.run(() => {
					//get the place result
                    let place: google.maps.places.PlaceResult = autocomplete.getPlace();
                    console.log(place);
                    console.log(this.filterForm.controls);
					//verify result
                    if (place.geometry === undefined || place.geometry === null) {
                        this.filterForm.controls["latitude"].setValue(null);
                        this.filterForm.controls["longitude"].setValue(null);
						return;
					}
					//set latitude, longitude and zoom
					this.filterForm.controls["latitude"].setValue(place.geometry.location.lat());
                    this.filterForm.controls["longitude"].setValue(place.geometry.location.lng());
                    this.geocodingService.getAddress(this.filterForm.controls["latitude"].value, this.filterForm.controls["longitude"].value).subscribe(response => {
                        this.filterForm.controls["address"].setValue(response);
                    });
                    this.isAddressValid = true;
				});
			});
		});	
	}
	
	//enters detail mode, sets the event to be displayed
	enterDetailMode(event: IEvent): void {
		this.event = event;
		this.isDetailMode = true;
	}
	
	//exits detail mode
	exitDetailMode(): void {
		this.isDetailMode = false;
	}
	
	//toggles displaying advanced search, triggered on click
	toggleAdvancedSearch(): void {
		this.isAdvancedSearch = !this.isAdvancedSearch;
	}
	
	//calls the http service and gets the events
	private getEvents(filter: IFilter): void {
		this.filter = filter;
		this.events = null;
//		this.isLoadingData = true;
		this.dataServiceSubscription = this._eventService.getEvents(filter)
            .subscribe(result => {
                this.events = result;
                this._loaderService.displayLoader(false);
            }, error => this.errorMessage = <any>error);
	}
	
	//gets the events when user checks the ascending order checkbox
	
	getEventsAscendingChanged(isAscending: boolean){
		let newFilter = this.filter;
		newFilter.OrderIsAscending = !newFilter.OrderIsAscending;
		//get the events
        this.getEvents(newFilter);
	}
	
	//gets the events when user changes the sorting order
	getEventsOrderChanged(newOrder: string){
		let newFilter = this.filter;
		newFilter.OrderByString = newOrder
		this.getEvents(newFilter);
	}
	
	toggleMapMode(): void {
		this.isMapMode = !this.isMapMode;
	}
	
	//creates the form for advanced search
	private createForm(): void {
        this.start = new FormControl(null);
        this.end = new FormControl(null);
        this.address = new FormControl(null);
        this.radius = new FormControl(null, Validators.pattern(/^[0-9]+(\.\d+)?$/));
        this.searchString = new FormControl(null);
        this.latitude = new FormControl(null);
        this.longitude = new FormControl(null);
        this.price = new FormControl(null, Validators.pattern(/^[0-9]+(\.\d{1,2})?$/));
        this.rating = new FormControl(null, Validators.pattern(/(^[1-4](\.\d+)?|5)$/));

        this.filterForm = new FormGroup({
            start: this.start,
            end: this.end,
            address: this.address,
            radius: this.radius,
            searchString: this.searchString,
            latitude: this.latitude,
            longitude: this.longitude,
            price: this.price,
            rating: this.rating
        });
        this.filterForm.setValidators([needBothOrNeitherOfAddressAndRadius('latitude', 'radius'), endDateBeforeStartDate('start', 'end')]);
    }

    clearLocation(): void {
        this.isAddressValid = false;
        this.filterForm.controls["address"].setValue("");
        this.filterForm.controls["latitude"].setValue(null);
        this.filterForm.controls["longitude"].setValue(null);
    }
}