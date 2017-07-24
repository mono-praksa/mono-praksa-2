import { Component } from '@angular/core'
import { IEvent } from '../models/event.model'

@Component({
    templateUrl: "app/components/event/views/event-create.component.html",
    styleUrls: ['app/components/event/views/event-create.component.css']
})
export class EventCreateComponent {
    createdEvent: IEvent;
    customizedEvent: IEvent;

    createEvent(event: IEvent) {
        this.createdEvent = event;
    }

    customizeEvent(event: IEvent) {
        this.customizedEvent = event;
    }
}