import { Component, Input } from "@angular/core";
import { DatePipe } from "@angular/common";

import { Event } from "../shared/models/event.model";
import { Filter } from "../shared/models/filter.model";

@Component({
    selector: "display-list",
    templateUrl:"app/event/event-search/event-search-list.component.html"
})

export class EventListComponent {
    @Input() events: Event[];
    @Input() filter: Filter;

    private onPageChange(event: any) {
        this.filter.PageNumber = event.page + 1;
        this.getEvents(filter);
    }
}