import { Component, DoCheck, ElementRef, NgZone, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { MapsAPILoader } from "@agm/core";
import { Subscription } from "rxjs/Subscription";

import { CategoryService } from "../shared/category.service";
import { CustomAttribute, Event } from "../shared/models/event.model";
import { endDateBeforeStartDate, needBothOrNeitherOfAddressAndRadius } from "../shared/validator";
import { EventService } from "../shared/event.service";
import { Filter } from "../shared/models/filter.model";
import { GeocodingService } from "../../shared/geocoding.service";
import { LoaderService } from "../../shared/loader.service";

@Component({
    styleUrls: ["./event-search.component.css"],
	templateUrl: "./event-search.component.html"
})
export class EventSearchComponent implements OnInit {

    address: FormControl;
    customCategoryName: FormControl;
    customCategoryValue: FormControl;
    filterForm: FormGroup;
    end: FormControl;
    isAddressValid: boolean = false;
    latitude: FormControl;
    longitude: FormControl;
    price: FormControl;
    radius: FormControl;
    rating: FormControl;
	@ViewChild("search") searchElementRef: ElementRef;
    searchString: FormControl;
    start: FormControl;

	private dataServiceSubscription: Subscription;
    private eventCount: number;
    private errorMessage: string;
    private events: Event[];
	private filter: Filter;
    private isAdvancedSearch: boolean = false;
    private isMapMode: boolean = false;
    private searchEventLoading: boolean = false;
    private userApproximateAddress: string = "";

    constructor(
        private categoryService: CategoryService,
        private eventService: EventService,
        private geocodingService: GeocodingService,
        private loaderService: LoaderService,
        private mapsAPILoader: MapsAPILoader,
        private ngZone: NgZone,
        private route: ActivatedRoute
    ) {
		this.createForm();
	}

    ngOnInit(): void {
        this.categoryService.buildCategories('category', false);

        this.geocodingService.getUserApproximateAddress()
            .subscribe(response => {
                if (response.status == "success") {
                    this.userApproximateAddress = response.city + ", " + response.country;
                }
            });

        this.loaderService.loaderStatus.subscribe((value: boolean) => {
            this.searchEventLoading = value;
        });
        
        if (this.route.snapshot.queryParams["searchString"]) {
            this.loaderService.displayLoader(true);
            this.searchString.setValue(this.route.snapshot.queryParams["searchString"]);
            this.filter = {
				ULat: undefined,
                ULong: undefined,
				Radius: 0,
                StartTime: undefined,
                EndTime: undefined,
                Category: 0,
                Price: undefined,
                RatingEvent: undefined,
                SearchString: this.route.snapshot.queryParams["searchString"],
                Custom: undefined,

				PageNumber: 1,
				PageSize: 25,
				OrderByString: "Name",
				OrderIsAscending: true
            }
            this.getEvents(this.filter);
		}
		else{
			this.isAdvancedSearch = true;
        }

        this.startMapZoomListener();
	}

    private clearLocation(): void {
        this.isAddressValid = false;
        this.filterForm.controls["address"].setValue("");
        this.filterForm.controls["latitude"].setValue(null);
        this.filterForm.controls["longitude"].setValue(null);
    }

    private clearStartTime(): void {
        this.filterForm.controls["start"].setValue("");
    }

    private clearEndTime(): void {
        this.filterForm.controls["end"].setValue("");
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
        this.customCategoryName = new FormControl(null);
        this.customCategoryValue = new FormControl(null);

        this.filterForm = new FormGroup({
            start: this.start,
            end: this.end,
            address: this.address,
            radius: this.radius,
            searchString: this.searchString,
            latitude: this.latitude,
            longitude: this.longitude,
            price: this.price,
            rating: this.rating,
            customCategoryName: this.customCategoryName,
            customCategoryValue: this.customCategoryValue
        });
        this.filterForm.setValidators([needBothOrNeitherOfAddressAndRadius("latitude", "radius"), endDateBeforeStartDate("start", "end")]);
    }

	//calls the http service and gets the events
	private getEvents(filter: Filter): void {
		this.dataServiceSubscription = this.eventService.getEvents(filter)
            .subscribe(result => {
                this.events = result.data;
                this.eventCount = result.metaData.TotalItemCount;
                this.loaderService.displayLoader(false);
            }, error => this.errorMessage = <any>error);
	}
	
	//gets the events when user checks the ascending order checkbox
    private getEventsAscendingChanged() {
        this.filter.OrderIsAscending = !this.filter.OrderIsAscending;
		//get the events
        this.getEvents(this.filter);
	}
	
	//gets the events when user changes the sorting order
    private getEventsOrderChanged(newOrder: string) {
		this.filter.OrderByString = newOrder
		this.getEvents(this.filter);
    }

	//return a number that represents the chosen categories
	//if none are selected it treats it as if all were selected
	private getSelectedCategories(): number {
        let chosenCategories: number[] = [];
		
        this.categoryService.categories.filter(checkbox => {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });
        var cat = 0
        for (let c of chosenCategories) {
            cat += c
        }
        return cat;
	}

    private onPageChange(event: any) {
        this.onSubmit(this.filterForm.value, event.page + 1, false);
    }

	//submits
    private onSubmit(formValues: any, pageNumber: number = 1, filterChanged: boolean = true): void {
        this.loaderService.displayLoader(true);
        let selectedCategories = this.getSelectedCategories();

        if (filterChanged) {
            let customModel: CustomAttribute[] = [{ key: formValues.customCategoryName, values: [formValues.customCategoryValue] }];
            let custom: string = null;
            if (customModel[0].key) {
                custom = JSON.stringify(customModel);
            }

            this.filter = {
                ULat: formValues.latitude,
                ULong: formValues.longitude,
                Radius: formValues.radius,
                StartTime: formValues.start,
                EndTime: formValues.end,
                Category: selectedCategories,
                SearchString: formValues.searchString,
                Price: formValues.price,
                RatingEvent: formValues.rating,
                Custom: custom,


                PageNumber: 1,
                PageSize: 25,
                OrderByString: "Name",
                OrderIsAscending: true
            }

            if (this.filter.SearchString == null) {
                this.filter.SearchString == "";
            }
        }
        else {
            this.filter.PageNumber = pageNumber;
        }
        		
        this.getEvents(this.filter);
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
    //is sets the latitude and longitude, then Zooms the map on the appropriate position, or so I"ve been told... 
    private startMapZoomListener() {
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
	
	//toggles displaying advanced search, triggered on click
	private toggleAdvancedSearch(): void {
		this.isAdvancedSearch = !this.isAdvancedSearch;
    }

	private toggleMapMode(): void {
		this.isMapMode = !this.isMapMode;
    }

	//called when the checkbox for one of the categories changes
	//updates the array of categories (i think)
    private updateCategories(category: number) {
        this.categoryService.categories.filter(checkbox => {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    }
}