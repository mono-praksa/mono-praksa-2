import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { ILocation } from "../models/location.model";

@Injectable()
export class LocationService {
    constructor(private _http: Http) { }

    getLocation(address: string): Observable<ILocation> {
        return this._http.get("/api/locations/get?address=" + address)
            .map((response: Response) => <ILocation>response.json())
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}