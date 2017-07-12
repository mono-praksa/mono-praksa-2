import { Component } from '@angular/core'
import { IEvent } from './../models/event.model'

@Component({
    template: `
        <create-event (eventEmitter)="eventEmitted($event)" *ngIf="!createdEvent"></create-event>
        <create-images *ngIf="createdEvent" [createdEvent]="createdEvent"></create-images>
    `
})
export class CreateComponent {
    createdEvent: IEvent;

    eventEmitted(event: IEvent) {
        this.createdEvent = event;
    }
}