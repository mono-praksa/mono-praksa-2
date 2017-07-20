﻿import { Component, Output, EventEmitter, NgZone, Input, OnInit } from '@angular/core'
import { IEvent } from '../models/event.model'
import { MapsAPILoader } from '@agm/core'
import { Observable } from 'rxjs/Observable'

import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'

import { Router } from '@angular/router'


declare var google: any;
var MarkerClusterer = require('./markerclusterer.js');

@Component({
    selector: 'display-map',
    styles: [`
    #map {
      height: 500px;
    }
    
  `],
    template: `
    <div id="map"></div>
  `
})

export class EventMapComponent implements OnInit {
    @Input() events: IEvent[]
    @Input() latitude: number
    @Input() longitude: number
    private _zoom: number;
    private _lat: number;
    private _lng: number;
    @Output() event = new EventEmitter()
    googleMarkers: any;
    map: any;

    constructor(private http: Http, private mapsAPILoader: MapsAPILoader, private ngZone: NgZone, private router: Router) {

    }

    ngOnInit() {

        //this sets the latitude and longitude form input
        if (this.latitude) {
            this.lat = this.latitude;
        }
        else {
            this.lat = 0;
        }
        if (this.longitude) {
            this.lng = this.longitude;
        }
        else {
            this.lng = 0;
        }

        //create a new object containing lat and lng
        let myLatLng = { lat: this.lat, lng: this.lng };

        //create the map
        //this searches the template for a div with the id "map"
        //sets the zoom and centers the map on the latitude and longitude from the input
        this.map = new google.maps.Map(document.getElementById('map'), {
            zoom: 8,
            center: myLatLng
        });

        //this initializes markers for the marker clusterer
        this.googleMarkers = this.initMarkers();

        //initializes the marker clusterer
        //this.map is the map where we will place the markers
        //this.googlemarkers are the markers that will be clustered
        //imagepath is the path to images for the cluster icons
        var mc = new MarkerClusterer(this.map, this.googleMarkers, { imagePath: 'https://googlemaps.github.io/js-marker-clusterer/images/m' });
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
            let myLatLng = { lat: markers[i].Lat, lng: markers[i].Long };

            //create a new marker

            let markerdata: IEvent = markers[i];

            var marker = new google.maps.Marker({
                map: self.map,
                position: myLatLng,
                title: markers[i].Name,
                data: markerdata
            });

            //create a new event listerer for the marker, triggers on click
            //displays the infowindow
            google.maps.event.addListener(marker, 'click', (function (marker: any, i: any) {
                return function () {
                    //
                    //once this is merged and routing is implemented this will redirect to event details
                    //
                    infoWindow.setContent('<div><p>' + marker.title + '</p><p>Double click for more</p><a href="home/">click me to go home</a></div>');
                    infoWindow.open(this.map, marker);
                }
            })(marker, i));

            //
            //once this is merged and router is implemented remove this function
            //
            //create a new event listener for the makrer, triggers on dblclick
            //calls the function that emits the marker to the parent component
            google.maps.event.addListener(marker, 'dblclick', (function (marker: any, i: any) {
                return function () {
                    let eventToEmit: IEvent = marker.data;
                    self.displayEvent(eventToEmit);
                }
            })(marker, i));

            //push the marker to the results
            result.push(marker);
        }
        //return the markers
        return result;
    }

    displayEvent(evt: IEvent) {
        this.event.emit(evt);
    }

    get zoom(): number {
        return this._zoom;
    }

    set zoom(theZoom: number) {
        this._zoom = theZoom;
    }

    get lat(): number {
        return this._lat;
    }

    set lat(theLatitude: number) {
        this._lat = theLatitude;
    }

    get lng(): number {
        return this._lng;
    }

    set lng(theLongitude: number) {
        this._lng = theLongitude;
    }
}