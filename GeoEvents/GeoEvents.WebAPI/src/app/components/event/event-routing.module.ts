import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { EventSearchComponent } from "./components/event-search.component";
import { EventCreateComponent } from "./components/event-create.component";

@NgModule({
	imports: [
		RouterModule.forChild([
			{ path: "event/search", component: EventSearchComponent },
			{ path: "event/create", component: EventCreateComponent }
		]),
	],
	exports: [
		RouterModule
	]
})
export class EventRoutingModule {}