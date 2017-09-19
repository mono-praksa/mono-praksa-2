import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Image } from "./models/image.model";

const API_URL = "2017-group1/api";

@Injectable()
export class ImageService {
    constructor(private http: Http) { }

    createImage(image: Image): Observable<Image> {
        return this.http.post(API_URL + "/images/create/" + image.EventId, image.FormData)
            .map((response: Response) => response.json());
    }

    getImages(id: string): Observable<Image[]> {
        return this.http.get(API_URL + "/images/get/" + id)
            .map((response: Response) => {
                return <Image[]>response.json();
            }).catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}