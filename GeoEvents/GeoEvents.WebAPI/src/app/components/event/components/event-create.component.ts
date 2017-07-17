import { Component } from '@angular/core'
import { IEvent } from '../models/event.model'

@Component({
    template: `
        <create-event (eventEmitter)="eventEmitted($event)" *ngIf="!createdEvent || _skip"></create-event>
        <create-images *ngIf="createdEvent && !_skip" (emittSkip)="skip($event)" [createdEvent]="createdEvent"></create-images>
    `
})
export class EventCreateComponent {
    createdEvent: IEvent;
    _skip: boolean = false;

    eventEmitted(event: IEvent) {
        this.createdEvent = event;
    }

    skip(variable: boolean) {
        this._skip = variable;
    }
}