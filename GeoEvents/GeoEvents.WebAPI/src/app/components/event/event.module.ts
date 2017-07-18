import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ReactiveFormsModule } from "@angular/forms";
import { EventRoutingModule } from "./event-routing.module";
import { NguiDatetimePickerModule } from '@ngui/datetime-picker';
import { AgmCoreModule } from '@agm/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';

import { EventSearchComponent } from "./components/event-search.component";
import { EventListComponent } from "./components/event-search-list.component";
import { EventMapComponent } from "./components/event-search-map.component";
import { EventCreateComponent } from "./components/event-create.component";
import { EventCreateImagesComponent } from "./components/event-create-images.component";
import { EventCreateDataComponent } from "./components/event-create-data.component";
import { EventDetailComponent } from "./components/event-detail.component";

import { EventService } from "./event.service";
import { PreserveSearchQuerryService } from "../../shared/preserve-search-querry.service";
import { GeocodingService } from '../../shared/geocoding.service';

@NgModule({
    imports: [
		EventRoutingModule,
		ReactiveFormsModule,
		NguiDatetimePickerModule,
		CommonModule,
		BrowserModule,
		AgmCoreModule.forRoot({
			apiKey: 'AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w',
			libraries: ['places']
		})
    ],

    declarations: [
		EventSearchComponent,
		EventListComponent,
		EventMapComponent,
		EventCreateComponent,
		EventCreateImagesComponent,
		EventCreateDataComponent,
		EventDetailComponent
	],
	
	providers: [ 
		EventService,
        PreserveSearchQuerryService,
        GeocodingService
	]
})
export class EventModule { }