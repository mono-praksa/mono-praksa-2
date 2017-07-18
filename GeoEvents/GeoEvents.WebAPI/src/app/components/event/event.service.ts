import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { IEvent } from "./models/event.model";
import { IFilter } from "./models/filter.model";

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

        if (filter.SearchString != null) {
            if (filter.SearchString.toString() != "") {
                querry += '&searchString=' + filter.SearchString.toString();
            }
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
	
	handleError(error: Response) {
		console.error(error);
		return Observable.throw(error.json().error || "Server error");
	}
}