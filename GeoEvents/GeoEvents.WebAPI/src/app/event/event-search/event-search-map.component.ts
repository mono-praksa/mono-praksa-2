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
    private initialized: boolean = false;
    private initialZoom: number = 1;
    private latitude: number;
    private longitude: number;
    private map: any;
    private mapPoints: MapPoint[];

    constructor(private router: Router, private eventService: EventService, private changeDetectorRef: ChangeDetectorRef) {

    }
	
	//called on detecting changes. seth the  clustering filter and the filterđ
	//then calls the service and retrieves the clusters from the database
    ngOnChanges() {
        if (!this.clusteringFilter) {
            this.clusteringFilter = {
                NELatitude: undefined,
                NELongitude: undefined,
                SWLatitude: undefined,
                SWLongitude: undefined,
                ZoomLevel: undefined
            }
        }

        if (!this.filter) {
            this.filter = {
                Category: undefined,
                Custom: undefined,
                EndTime: undefined,
                OrderByString: undefined,
                OrderIsAscending: undefined,
                PageNumber: undefined,
                PageSize: undefined,
                Price: undefined,
                Radius: undefined,
                RatingEvent: undefined,
                SearchString: undefined,
                StartTime: undefined,
                ULat: undefined,
                ULong: undefined
            }
        }

        this.latitude = this.filter.ULat;
        this.longitude = this.filter.ULong;
        this.initialZoom = this.getZoom(this.filter.Radius);
        if (!this.initialized) {
            this.initialized = true;
        }
        else {
            this.getEvents();
        }
    }

	//called once on init. sets the clustering filter to default values. sets the filter to default values
	//initializes the map, and retrieves the events from the database.
    ngOnInit() {
        this.initMap();
    }
	
	//check wether there is an infowindow open. if there is an infowindow open, the function closes it.
	//triggered when a marker is clicked. 
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

	//gets the event on markerClick and redirects to details for the selected event
    private displayEvent(evt: Event): void {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

	//calls service and retrieves the events from the database
    private getEvents(): void {
        this.eventService.getEventsClustered(this.filter, this.clusteringFilter).subscribe((response: MapPoint[]) => {
            this.mapPoints = response;
        })
    }

	//gets the iconurl based on the number of markers inside a cluster
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

	//Calculates the zoom level of the map based on the radius of the filter
    private getZoom(radius: number): number {
        if (radius) {
            return Math.ceil((16 - Math.log(radius * 2) / Math.log(2)));
        }
        else {
            return 1;
        }
    }
	
	//returns the radius based on the zoom level
    private getRadius(zoom: number = 1): number {
        return Math.pow(2, 15 - zoom);
    }

	//initializes the map and sets the initial latitude, longitude and zoom level
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

	//sets the map once it is initialized and ready.
    private mapReady(event: any): void {
        this.map = event;
    }

	//called when clicked on a cluster, centers the map on the clicked cluster and increases the zoom level
    private markerClick(mapPoint: MapPoint): void {
        this.latitude = mapPoint.Y;
        this.longitude = mapPoint.X;
        this.initialZoom = 2 + this.map.zoom;
    }

	//sets the bounds in the clustering filter when user pans/zooms and bounds change
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

	//sets the zoom level of the clustering filter once the user zooms
    private onZoomChange(zoom: number): void {
        this.clusteringFilter.ZoomLevel = zoom;
        this.filter.Radius = this.getRadius(zoom);
    }
}