﻿import { Component, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MapsAPILoader } from "@agm/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { ClusteringFilter } from "../shared/models/clustering-filter.model";
import { Event } from "../shared/models/event.model";
import { EventService } from "../shared/event.service";
import { Filter } from "../shared/models/filter.model";
import { MapPoint } from "../shared/models/map-point.model";

@Component({
    selector: "display-map",
    styleUrls: ["app/event/event-search/event-search-map.component.css"],
    templateUrl: "app/event/event-search/event-search-map.component.html"
})

export class EventMapComponent implements OnChanges, OnInit {
    @Input() filter: Filter;

    private clusteringFilter: ClusteringFilter;
    private latitude: number;
    private longitude: number;
    private mapPoints: MapPoint[];
    private zoom: number = 3;

    constructor(private router: Router, private eventService: EventService) {

    }

    ngOnChanges() {
        if (this.filter) {
            this.clusteringFilter = {
                NELatitude: 50.5,
                NELongitude: 50.5,
                SWLatitude: 10.2,
                SWLongitude: 10.2,
                ZoomLevel: 4
            }
            this.eventService.getEventsClustered(this.filter, this.clusteringFilter).subscribe((response: MapPoint[]) => {
                this.mapPoints = response;
            })
        }
    }

    ngOnInit() {
        this.initMap();
        if (this.filter) {
            this.clusteringFilter = {
                NELatitude: 50.5,
                NELongitude: 50.5,
                SWLatitude: 10.2,
                SWLongitude: 10.2,
                ZoomLevel: 4
            }
            this.eventService.getEventsClustered(this.filter, this.clusteringFilter).subscribe((response: MapPoint[]) => {
                this.mapPoints = response;
            })
        }        
    }

    private displayEvent(evt: Event): void {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

    private getZoom(radius: number): number {
        if (radius) {
            return (16 - Math.log(radius / 500) / Math.log(2));
        }
        else {
            return 3;
        }
    }

    private initMap(): void {
        //this sets the latitude and longitude form input
        if (this.filter) {
            this.latitude = this.filter.ULat;
            this.longitude = this.filter.ULong;
            this.zoom = this.getZoom(this.filter.Radius);
        }
        if (this.mapPoints && this.mapPoints.length > 0 && !this.latitude) {
            this.latitude = this.mapPoints[0].Y;
            this.longitude = this.mapPoints[0].X;
        }
        else {
            this.latitude = 0;
            this.longitude = 0;
        }
    }
}