import { Component, OnInit, ElementRef, NgZone, ViewChild } from '@angular/core'
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'
import { MapsAPILoader } from '@agm/core'

@Component({
    templateUrl: "app/search-events.component.html"
})
export class SearchEventsComponent implements OnInit {

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

    mapMode: boolean = false

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
}