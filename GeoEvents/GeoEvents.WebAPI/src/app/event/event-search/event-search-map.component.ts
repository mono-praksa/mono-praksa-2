﻿import { Component, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Event } from "../shared/models/event.model";
import { Filter } from "../shared/models/filter.model";


declare var google: any;
var MarkerClusterer = require("../../../../Scripts/markerclusterer.js");

@Component({
    selector: "display-map",
    styleUrls: ["app/event/event-search/event-search-map.component.css"],
    templateUrl: "app/event/event-search/event-search-map.component.html"
})

export class EventMapComponent implements OnInit, OnChanges {
    @Input() events: Event[];
    @Input() filter: Filter;
    googleMarkers: any;
    @Input() latitude: number;
    @Input() longitude: number;
    map: any;

    private lat: number;
    private lng: number;
    private zoom: number;

    constructor(private router: Router) {

    }

    displayEvent(evt: Event) {
        let routeUrl = "/event/search/" + evt.Id;
        this.router.navigate([routeUrl]);
    }

    //initializes the markers
    initMarkers() {
        let i = 0;
        let markers = this.events;
        var result = [];

        //set the scope
        var self = this;

        //infowindow that is shown on click
        var infoWindow = new google.maps.InfoWindow();
        //craete google markers for each event
        for (; i < markers.length; ++i) {
            //set the coordinates
            let myLatLng = { lat: markers[i].Latitude, lng: markers[i].Longitude };

            //create a new marker

            let markerdata: Event = markers[i];

            var marker = new google.maps.Marker({
                map: self.map,
                position: myLatLng,
                title: markers[i].Name,
                data: markerdata
            });

            //create a new event listerer for the marker, triggers on click
            //displays the infowindow
            google.maps.event.addListener(marker, "click", (function (marker: any, i: any) {
                return function () {
                    //
                    //once this is merged and routing is implemented this will redirect to event details
                    //
                    infoWindow.setContent("<div><p>" + marker.title + "</p> <p>Double-click for more!</p></div>");
                 //   self.MarkerLink.addListener("click", self.router.navigate(["/event/create"]));
                    infoWindow.open(this.map, marker);
                }
            })(marker, i));

            //
            //once this is merged and router is implemented remove this function
            //
            //create a new event listener for the makrer, triggers on dblclick
            //calls the function that emits the marker to the parent component
			
			
            google.maps.event.addListener(marker, "dblclick", (function (marker: any, i: any) {
                return function () {
                    let eventToEmit: Event = marker.data;
                    self.displayEvent(eventToEmit);
                }
            })(marker, i));
			

            //push the marker to the results
            result.push(marker);
        }
        //return the markers
        return result;
    }

    ngOnChanges() {
        this.map = undefined;
        this.googleMarkers = undefined;

        if (this.events && this.events.length > 0) {
            if (this.latitude) {
                this.lat = this.latitude;
            }
            else {
                this.lat = this.events[0].Latitude;
            }
            if (this.longitude) {
                this.lng = this.longitude;
            }
            else {
                this.lng = this.events[0].Longitude;
            }
        }
        else {
            this.lat = 0;
            this.lng = 0;
        }

        //create a new object containing lat and lng
        let myLatLng = { lat: this.lat, lng: this.lng };

        //create the map
        //this searches the template for a div with the id "map"
        //sets the zoom and centers the map on the latitude and longitude from the input
        this.map = new google.maps.Map(document.getElementById("map"), {
            zoom: 3,
            center: myLatLng,
            minZoom: 3
        });

        //this initializes markers for the marker clusterer
        if (this.events && this.events.length > 0) {
            this.googleMarkers = this.initMarkers();

            //initializes the marker clusterer
            //this.map is the map where we will place the markers
            //this.googlemarkers are the markers that will be clustered
            //imagepath is the path to images for the cluster icons
            var mc = new MarkerClusterer(this.map, this.googleMarkers, { maxZoom: 15, imagePath: "https://googlemaps.github.io/js-marker-clusterer/images/m" });
        }
    }

    ngOnInit() {

        //this sets the latitude and longitude form input
        if (this.events && this.events.length > 0) {
            if (this.latitude) {
                this.lat = this.latitude;
            }
            else {
                this.lat = this.events[0].Latitude;
            }
            if (this.longitude) {
                this.lng = this.longitude;
            }
            else {
                this.lng = this.events[0].Longitude;
            }
        }
        else {
            this.lat = 0;
            this.lng = 0;
        }

        //create a new object containing lat and lng
        let myLatLng = { lat: this.lat, lng: this.lng };

        //create the map
        //this searches the template for a div with the id "map"
        //sets the zoom and centers the map on the latitude and longitude from the input
        this.map = new google.maps.Map(document.getElementById("map"), {
            zoom: 3,
            center: myLatLng,
			minZoom: 3
        });

        //this initializes markers for the marker clusterer
        if (this.events && this.events.length > 0) {
            this.googleMarkers = this.initMarkers();

            //initializes the marker clusterer
            //this.map is the map where we will place the markers
            //this.googlemarkers are the markers that will be clustered
            //imagepath is the path to images for the cluster icons
            var mc = new MarkerClusterer(this.map, this.googleMarkers, { maxZoom: 15, imagePath: "https://googlemaps.github.io/js-marker-clusterer/images/m" });
        }
    }
}