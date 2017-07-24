import { Injectable } from "@angular/core"
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router'
import { EventService } from './event.service'
import { IEvent } from './models/event.model'
import { Observable } from 'rxjs/Observable'

@Injectable()
export class EventDetailRouteActivatorService implements CanActivate {
    constructor(private eventService: EventService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot) {
        if (!this.isGuid(route.params.eventId)) {
            this.router.navigate(['../../../404']);
        }
        
        this.eventService.getEventById(route.params.eventId).subscribe((event: IEvent) => {
            if (event.Id === "00000000-0000-0000-0000-000000000000") {
                this.router.navigate(['../../../404']);
            } else {
                this.router.routerState.root.data['event'] = event;
            }
        })
        return true;
    }

    isGuid(id: string) {
        var pattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
        return pattern.test(id);
    }
}