import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Event } from "../shared/models/event.model";
import { EventService } from "../shared/event.service";

@Injectable()
export class EventDetailResolverService implements Resolve<Event> {
    constructor(private eventService: EventService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.eventService.getEventById(route.params.eventId);
    }
}