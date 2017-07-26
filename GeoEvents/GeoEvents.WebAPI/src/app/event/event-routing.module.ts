import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { EventCreateComponent } from "./event-create/event-create.component";
import { EventDetailComponent } from "./event-detail/event-detail.component";
import { EventDetailResolverService } from "./event-detail/event-detail-resolver.service"
import { EventDetailRouteActivatorService } from "./event-detail/event-detail-route-activator.service";
import { EventSearchComponent } from "./event-search/event-search.component";

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