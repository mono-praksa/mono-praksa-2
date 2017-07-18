import { Injectable, OnInit } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/observable';
import { IEvent } from '../components/event/models/event.model';

@Injectable()
export class GeocodingService implements OnInit {
    _userApproximateAddress: string = "";

    constructor(private http: Http) {}

    get userApproximateAddress(): string {
        return this._userApproximateAddress;
    }

    set userApproximateAddress(thisUserApproximateAddress: string) {
        this._userApproximateAddress = thisUserApproximateAddress;
    }

    ngOnInit() {
        this.getUserApproximateAddress().subscribe((response: any) => this.userApproximateAddress = response);
    }

    private getUserApproximateAddress() {
        return this.http.get("http://ip-api.com/json")
            .map((response: Response) => {
                return response.json();
            }).catch(this.handleError);
    }

    getAddress(latitude: number, longitude: number) : Observable<string>{
        return this.http.get('https://maps.googleapis.com/maps/api/geocode/json?latlng=' + latitude.toString() + ',' + longitude.toString() + '&key=AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w')
            .map((response: Response) => {
                return <string>response.json()["results"][0]["formatted_address"];
            }).catch(this.handleError);
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }
}