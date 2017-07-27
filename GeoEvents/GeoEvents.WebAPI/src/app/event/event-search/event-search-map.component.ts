﻿import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MapsAPILoader } from "@agm/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Event } from "../shared/models/event.model";
import { Filter } from "../shared/models/filter.model";

@Component({
    selector: "display-map",
    styleUrls: ["app/event/event-search/event-search-map.component.css"],
    templateUrl: "app/event/event-search/event-search-map.component.html"
})

export class EventMapComponent implements OnInit {
    @Input() events: Event[];
    @Input() filter: Filter;

    private latitude: number;
    private longitude: number;
    private markers: any[] = [];
    private zoom: number;

    constructor(private router: Router) {

    }

    ngOnInit() {
        this.initMap();
    }

    private displayEvent(evt: Event): void {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

    private getZoom(): number {
        return 2;
    }

    private initMap(): void {
        //this sets the latitude and longitude form input
        if (this.filter) {
            this.latitude = this.filter.ULat;
            this.longitude = this.filter.ULong;
        }
        if (this.events && this.events.length > 0 && !this.latitude) {
            this.latitude = this.events[0].Latitude;
            this.longitude = this.events[0].Longitude;
        }
        else {
            this.latitude = 0;
            this.longitude = 0;
        }
    }
}