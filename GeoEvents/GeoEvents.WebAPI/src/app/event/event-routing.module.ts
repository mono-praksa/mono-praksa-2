import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { EventSearchComponent } from "./components/event-search.component";
import { EventCreateComponent } from "./components/event-create.component";
import { EventDetailComponent } from './components/event-detail.component';
import { EventDetailRouteActivatorService } from './providers/event-detail-route-activator.service';
import { EventDetailResolverService } from './providers/event-detail-resolver.service'

@NgModule({
	imports: [
        RouterModule.forChild([
            { path: "event/search/:eventId", component: EventDetailComponent, canActivate: [EventDetailRouteActivatorService], resolve: { event: EventDetailResolverService } },
			{ path: "event/search", component: EventSearchComponent },
			{ path: "event/create", component: EventCreateComponent }
		]),
	],
	exports: [
		RouterModule
    ]
})
export class EventRoutingModule {}