﻿import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'

@Component({
    selector: "create-event",
    templateUrl: "app/create-event.component.html"
})
export class CreateEventComponent implements OnInit {
    categories: any[] = [
        { id: 1, checked: false },
        { id: 2, checked: false },
        { id: 4, checked: false },
        { id: 8, checked: false },
        { id: 16, checked: false },
        { id: 32, checked: false },
        { id: 64, checked: false }
    ]
    eventForm: FormGroup

    name: FormControl
    description: FormControl
    start: FormControl
    end: FormControl
    category: FormControl
    address: FormControl

    constructor(private http: Http) { }

    ngOnInit() {
        this.name = new FormControl('', Validators.required);
        this.description = new FormControl('', Validators.required);
        this.start = new FormControl('', Validators.required);
        this.end = new FormControl('', Validators.required);
        this.address = new FormControl('', Validators.required);

        this.eventForm = new FormGroup({
            name: this.name,
            description: this.description,
            start: this.start,
            end: this.end,
            address: this.address
        });
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
            Lat: 45,
            Long: 45,
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
}