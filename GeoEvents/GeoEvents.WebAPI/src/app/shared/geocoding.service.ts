import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

@Injectable()
export class GeocodingService {
    userApproximateAddress: string = "";

    constructor(private http: Http) {}

    getAddress(latitude: number, longitude: number) : Observable<string>{
        return this.http.get("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude.toString() + "," + longitude.toString() + "&key=AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w")
            .map((response: Response) => {
                if (response.json().status === "ZERO_RESULTS") {
                    return "";
                }
                else {
                    return <string>response.json()["results"][0]["formatted_address"];
                }                
            }).catch(this.handleError);
    }

    getUserApproximateAddress() {
        return this.http.get("http://ip-api.com/json")
            .map((response: Response) => {
                return response.json();
            }).catch(this.handleError);
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }

    // get userApproximateAddress(): string {
    //     return this.userApproximateAddress;
    // }

    // set userApproximateAddress(thisUserApproximateAddress: string) {
    //     this.userApproximateAddress = thisUserApproximateAddress;
    // }
}