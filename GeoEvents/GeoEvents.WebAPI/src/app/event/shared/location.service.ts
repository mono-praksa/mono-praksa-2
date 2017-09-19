﻿import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Location } from "./models/location.model";

const API_URL = "2017-group1/api";

@Injectable()
export class LocationService {
    constructor(private http: Http) { }

    getLocation(address: string): Observable<Location> {
        return this.http.get(API_URL + "/locations/get?address=" + address)
            .map((response: Response) => <Location>response.json())
            .catch(this.handleError);
    }

    getLocationById(id: string): Observable<Location> {
        return this.http.get(API_URL + "/locations/get?id=" + id)
            .map((response: Response) => <Location>response.json())
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }

    updateRating(locationId: string, rating: number, currentRating: number, rateCount: number): Observable<Location> {
        return this.http.put(API_URL + "/locations/update/rating?locationId=" + locationId + "&rating=" + rating + "&currentrating=" + currentRating + "&ratecount=" + rateCount, {})
            .map((response: Response) => {
                return <Location>response.json();
            }).catch(this.handleError);
    }
}