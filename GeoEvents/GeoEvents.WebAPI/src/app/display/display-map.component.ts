﻿import { Component, Output, EventEmitter, NgZone, Input } from '@angular/core'
import { IEvent } from './../models/event.model'
import { MapsAPILoader } from '@agm/core'
import { Observable } from 'rxjs/Rx'

import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms'
import { Http, Response, Headers, RequestOptions } from '@angular/http'

import { Router } from '@angular/router'


@Component({
    selector: 'display-map',
    styles: [`
    agm-map {
      height: 500px;
    }
    
  `],
    template: `
    <agm-map [latitude]="latitude" [longitude]="longitude">
    <agm-marker *ngFor= "let evt of events" [latitude]="evt.Lat" [longitude]="evt.Long" (markerClick)="displayEvent(evt)" [title]="evt.Name + ' (See more)'"></agm-marker>
    </agm-map>
  `
})

export class DisplayMapComponent {
    @Input() events: IEvent[]
    @Input() radius: number
    @Input() latitude: number
    @Input() longitude: number
    zoom: number;
    lat: number;
    lng: number;
    @Output() event = new EventEmitter()

    constructor(private http: Http, private mapsAPILoader: MapsAPILoader, private ngZone: NgZone, private router: Router) {
        this.lat = this.latitude;
        this.lng = this.longitude;
        this.zoom = 16;
    }



    displayEvent(evt: IEvent) {
        this.event.emit(evt);

    }

}