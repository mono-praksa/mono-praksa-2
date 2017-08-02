import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Image } from "./models/image.model";

@Injectable()
export class ImageService {
    constructor(private http: Http) { }

    createImage(image: Image): Observable<Image> {
        return this.http.post("/api/images/create/" + image.EventId, image.FormData)
            .map((response: Response) => response.json());
    }

    getImages(id: string): Observable<Image[]> {
        return this.http.get("/api/images/get/" + id)
            .map((response: Response) => {
                return <Image[]>response.json();
            }).catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}