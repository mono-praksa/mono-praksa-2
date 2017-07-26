import { AgmCoreModule } from "@agm/core";
import { CommonModule, DecimalPipe } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { NguiDatetimePickerModule } from "@ngui/datetime-picker";
import { PaginatorModule } from "primeng/primeng";

import { EventCreateComponent } from "./event-create/event-create.component";
import { EventCreateDataComponent } from "./event-create/event-create-data/event-create-data.component";
import { EventCreateCustomizeComponent } from "./event-create/event-create-customize/event-create-customize.component";
import { EventCreateImagesComponent } from "./event-create/event-create-images/event-create-images.component";
import { EventDetailComponent } from "./event-detail/event-detail.component";
import { EventRoutingModule } from "./event-routing.module";
import { EventSearchComponent } from "./event-search/event-search.component";
import { EventListComponent } from "./event-search/event-search-list.component";
import { EventMapComponent } from "./event-search/event-search-map.component";

import { EventDetailResolverService } from "./event-detail/event-detail-resolver.service";
import { EventDetailRouteActivatorService } from "./event-detail/event-detail-route-activator.service";
import { CategoryService } from "./shared/category.service";
import { EventService } from "./shared/event.service";
import { LocationService } from "./shared/location.service";

import { GeocodingService } from "../shared/geocoding.service";
import { PreserveSearchQueryService } from "../shared/preserve-search-query.service";

@NgModule({
    imports: [
		AgmCoreModule.forRoot({
			apiKey: "AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w",
			libraries: ["places"]
        }),
		BrowserModule,
		CommonModule,
		EventRoutingModule,
		NguiDatetimePickerModule,
        PaginatorModule,
		ReactiveFormsModule
    ],

    declarations: [
		EventCreateComponent,
        EventCreateCustomizeComponent,
        EventCreateDataComponent,
		EventCreateImagesComponent,
		EventDetailComponent,
		EventListComponent,
		EventMapComponent,
		EventSearchComponent
	],
	
	providers: [ 
        CategoryService,
        EventDetailResolverService,
        EventDetailRouteActivatorService,
        EventService,
        GeocodingService,
        LocationService,
        PreserveSearchQueryService
	]
})
export class EventModule { }