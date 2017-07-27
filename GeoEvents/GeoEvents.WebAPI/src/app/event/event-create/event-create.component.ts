import { Component } from "@angular/core";
import { Event } from "../shared/models/event.model";

@Component({
    templateUrl: "app/event/event-create/event-create.component.html",
    styleUrls: ["app/event/event-create/event-create.component.css"]
})
export class EventCreateComponent {
    createdEvent: Event;
    customizedEvent: Event;

    createEvent(event: Event) {
        this.createdEvent = event;
    }

    customizeEvent(event: Event) {
        this.customizedEvent = event;
    }
}