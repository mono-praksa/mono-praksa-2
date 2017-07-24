import { Injectable } from '@angular/core'
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router'
import { EventService } from './event.service'
import { Observable } from 'rxjs/Observable'

import { IEvent } from './models/event.model'

@Injectable()
export class EventDetailResolverService implements Resolve<IEvent> {
    constructor(private eventService: EventService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.eventService.getEventById(route.params.eventId);
    }
}