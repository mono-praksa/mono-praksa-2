import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'

@Component({
    selector: "create-event",
    templateUrl: "app/create-event.component.html"
})
export class CreateEventComponent implements OnInit {
    eventForm: FormGroup

    name: FormControl
    description: FormControl
    start: FormControl
    end: FormControl
    category: FormControl
    address: FormControl

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

    createEvent(formValues: any) {
        console.log(formValues);
    }
}