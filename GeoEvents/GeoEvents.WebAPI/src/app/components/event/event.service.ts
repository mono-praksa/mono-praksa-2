import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { IEvent } from "./models/event.model";
import { IFilter } from "./models/filter.model";
import { IImage } from './models/image.model';

@Injectable()
export class EventService {
    constructor(private _http: Http) { }

    getEvents(filter: IFilter): Observable<IEvent[]> {
        let querry = "/api/events/search";

        querry += "?category=";
        querry += filter.Category.toString();

        if (filter.ULat != null && filter.ULong != null && filter.Radius != null && filter.Radius != 0) {
            querry += '&uLat=' + filter.ULat.toString();
            querry += '&uLong=' + filter.ULong.toString();
            querry += '&radius=' + filter.Radius.toString();
        }

        if (filter.StartTime != null && filter.StartTime.toString() != "") {
            querry += '&startTime=' + filter.StartTime.toString().replace(':', 'h');
        }

        if (filter.EndTime != null && filter.EndTime.toString() != "") {
            querry += '&endTime=' + filter.EndTime.toString().replace(':', 'h');
        }

        if (filter.Price != null && filter.Price >= 0) {
            querry += '&price=' + filter.Price.toString();
        }

        if (filter.RatingEvent != null && filter.RatingEvent >= 1 && filter.RatingEvent <= 5) {
            querry += '&ratingEvent=' + filter.RatingEvent.toString();
        }

        if (filter.SearchString != null) {
            if (filter.SearchString.toString() != "") {
                querry += '&searchString=' + filter.SearchString.toString();
            }
        }

        if (filter.Custom != null) {
            querry += '&custom=' + filter.Custom;
        }

        querry += '&pageNumber=' + filter.PageNumber.toString();
        querry += '&pageSize=' + filter.PageSize.toString();
        querry += '&orderAscending=' + filter.OrderIsAscending.toString();
        querry += '&orderBy=' + filter.OrderByString.toString();

        //execute http call
        return this._http.get(querry)
            .map((response: Response) => <IEvent[]>response.json())
            .catch(this.handleError);
    }

    createEvent(event: IEvent): Observable<IEvent> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post('/api/events/create', JSON.stringify(event), options)
            .map((response: Response) => <IEvent>response.json())
            .catch(this.handleError);
    }

    createImage(image: IImage): Observable<IImage> {
        let options = new RequestOptions();
        return this._http.post('/api/images/create/' + image.EventId, image.Content, options)
            .map((response: Response) => response.json())
            .catch(this.handleError);
    }

    getImages(id: string): Observable<IImage[]> {
        return this._http.get('/api/images/get/' + id)
            .map((response: Response) => {
                return <IImage[]>response.json();
            }).catch(this.handleError);
    }

    updateRating(eventId: string, rating: number, currentrating: number, ratecount: number) {
        return this._http.put("/api/events/update/rating?eventId=" + eventId + "&rating=" + rating+"&currentrating=" + currentrating + "&ratecount=" + ratecount, {})
            .map((response: Response) => {
                return <IEvent>response.json();
            }).catch(this.handleError);
    }

    updateReservation(eventId: string) {
        return this._http.put("/api/events/update/reservation?eventId=" + eventId, {})
            .map((response: Response) => {
                return <IEvent>response.json();
            }).catch(this.handleError);
    }
	
	handleError(error: Response) {
		console.error(error);
		return Observable.throw(error.json().error || "Server error");
	}
}