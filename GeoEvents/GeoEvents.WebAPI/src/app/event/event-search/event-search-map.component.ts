﻿import { Component, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { LatLngBounds, MapsAPILoader } from "@agm/core";
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
    private initialZoom: number = 1;

    constructor(private router: Router, private eventService: EventService) {

    }

    ngOnChanges() {
        if (this.filter) {
            this.getEvents();
        }
    }

    ngOnInit() {
        this.clusteringFilter = {
            NELatitude: undefined,
            NELongitude: undefined,
            SWLatitude: undefined,
            SWLongitude: undefined,
            ZoomLevel: undefined
        }
        this.initMap();
        if (this.filter) {
            this.getEvents();
        }
    }

    private displayEvent(evt: Event): void {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

    private getEvents(): void {
        console.log("getEvents");
        this.eventService.getEventsClustered(this.filter, this.clusteringFilter).subscribe((response: MapPoint[]) => {
            this.mapPoints = response;
        })
    }

    private getIconUrl(count: number): string {
        if (count === 1) {
            return "app/assets/images/pin.png";
        }
        else if (count < 10) {
            return "app/assets/images/m1.png";
        }
        else if (count < 30) {
            return "app/assets/images/m2.png";
        }
        else if (count < 100) {
            return "app/assets/images/m3.png";
        }
        else if (count < 500) {
            return "app/assets/images/m4.png";
        }
        else if (count >= 500) {
            return "app/assets/images/m5.png";
        }
        else {
            return "";
        }
}

    private getZoom(radius: number): number {
        if (radius) {
            return Math.ceil((16 - Math.log(radius * 2) / Math.log(2)));
        }
        else {
            return 1;
        }
    }

    private getRadius(zoom: number = 1): number {
        return Math.pow(2, 15 - zoom);
    }

    private initMap(): void {
        if (this.filter) {
            this.latitude = this.filter.ULat;
            this.longitude = this.filter.ULong;
            this.initialZoom = this.getZoom(this.filter.Radius);
        }
        else {
            this.latitude = 0;
            this.longitude = 0;
            this.initialZoom = this.getZoom(undefined);
        }
    }

    private markerClick(event: any): void {
        this.latitude = event.coords.latitude;
        this.longitude = event.coords.longitude;
        this.initialZoom = this.clusteringFilter.ZoomLevel + 1;
    }

    private onBoundsChange(bounds: LatLngBounds): void {
        var center = bounds.getCenter();
        var ne = bounds.getNorthEast();
        var sw = bounds.getSouthWest();
        this.filter.ULat = center.lat();
        this.filter.ULong = center.lng();
        this.clusteringFilter.NELatitude = ne.lat();
        this.clusteringFilter.NELongitude = ne.lng();
        this.clusteringFilter.SWLatitude = sw.lat();
        this.clusteringFilter.SWLongitude = sw.lng();
    }

    private onZoomChange(zoom: number): void {
        this.clusteringFilter.ZoomLevel = zoom;
        this.filter.Radius = this.getRadius(zoom);
    }
}