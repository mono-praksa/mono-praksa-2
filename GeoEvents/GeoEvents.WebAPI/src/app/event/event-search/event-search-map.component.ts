﻿import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from "@angular/core";
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
    private infoWindow: any;
    private initialZoom: number = 1;
    private latitude: number;
    private longitude: number;
    private map: any;
    private mapPoints: MapPoint[];

    constructor(private router: Router, private eventService: EventService, private changeDetectorRef: ChangeDetectorRef) {

    }

    ngOnChanges() {
        if (!this.clusteringFilter) {
            this.clusteringFilter = {
                NELatitude: 90,
                NELongitude: 180,
                SWLatitude: -90,
                SWLongitude: -180,
                ZoomLevel: 1
            }
        }

        if (!this.filter) {
            this.filter = {
                Category: 0,
                Custom: undefined,
                EndTime: undefined,
                OrderByString: undefined,
                OrderIsAscending: undefined,
                PageNumber: undefined,
                PageSize: undefined,
                Price: undefined,
                Radius: 25000,
                RatingEvent: undefined,
                SearchString: undefined,
                StartTime: undefined,
                ULat: 0,
                ULong: 0
            }
        }

        this.latitude = this.filter.ULat;
        this.longitude = this.filter.ULong;
        this.initialZoom = this.getZoom(this.filter.Radius);
        this.getEvents();
    }

    ngOnInit() {
        this.clusteringFilter = {
            NELatitude: 90,
            NELongitude: 180,
            SWLatitude: -90,
            SWLongitude: -180,
            ZoomLevel: 1
        }
        this.initMap();
        if (!this.filter) {
            this.filter = {
                Category: 0,
                Custom: undefined,
                EndTime: undefined,
                OrderByString: undefined,
                OrderIsAscending: undefined,
                PageNumber: undefined,
                PageSize: undefined,
                Price: undefined,
                Radius: 25000,
                RatingEvent: undefined,
                SearchString: undefined,
                StartTime: undefined,
                ULat: 0,
                ULong: 0
            }
        }
        
        this.getEvents();
    }

    private checkWindows(infoWindow: any): void {
        this.changeDetectorRef.detectChanges();
        if (this.infoWindow == infoWindow) {
            return;
        }
        else if (this.infoWindow) {
            this.infoWindow.close();
        }
        this.infoWindow = infoWindow;
        this.changeDetectorRef.detectChanges();
    }

    private displayEvent(evt: Event): void {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

    private getEvents(): void {
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

    private mapReady(event: any): void {
        this.map = event;
    }

    private markerClick(mapPoint: MapPoint): void {
        this.latitude = mapPoint.Y;
        this.longitude = mapPoint.X;
        this.initialZoom = 2 + this.map.zoom;
    }

    private onBoundsChange(bounds: LatLngBounds): void {
        this.infoWindow = undefined;
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