import { Component } from "@angular/core";
import { IEvent } from "../shared/models/event.model";

@Component({
    templateUrl: "app/event/event-create/event-create.component.html",
    styleUrls: ["app/event/event-create/event-create.component.css"]
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