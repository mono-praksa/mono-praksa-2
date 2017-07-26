import { Component, EventEmitter, Input, Output } from "@angular/core";
import { DatePipe } from "@angular/common";

import { Event } from "../shared/models/event.model";

@Component({
    selector: "display-list",
    templateUrl:"app/components/event/views/event-search-list.component.html"
})

export class EventListComponent {
    @Output() event = new EventEmitter();
    @Input() events: Event[];

    eventDetails(evt: Event) {
        this.event.emit(evt);
    }
}