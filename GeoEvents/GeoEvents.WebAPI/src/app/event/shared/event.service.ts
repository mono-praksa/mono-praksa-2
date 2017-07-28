import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { ClusteringFilter } from "./models/clustering-filter.model";
import { Event } from "./models/event.model";
import { Filter } from "./models/filter.model";
import { Image } from "./models/image.model";
import { MapPoint } from "./models/map-point.model";

@Injectable()
export class EventService {
    constructor(private http: Http) { }

    createEvent(event: Event): Observable<Event> {
        let headers = new Headers({ "Content-Type": "application/json" });
        let options = new RequestOptions({ headers: headers });
        return this.http.post("/api/events/create", JSON.stringify(event), options)
            .map((response: Response) => <Event>response.json())
            .catch(this.handleError);
    }

    createImage(image: Image): Observable<Image> {
        let options = new RequestOptions();
        return this.http.post("/api/images/create/" + image.EventId, image.FormData, options)
            .map((response: Response) => response.json());
    }

    getEventById(eventId: number): Observable<Event> {
        return this.http.get("/api/events/get?eventId=" + eventId)
            .map((response: Response) => <Event>response.json())
            .catch(this.handleError);
    }

    getEventCount(filter: Filter): Observable<number> {
        let query = "/api/events/search/count" + this.makeQueryString(filter);
        return this.http.get(query)
            .map((response: Response) => <number>response.json())
            .catch(this.handleError); 
    }

    getEvents(filter: Filter): Observable<any> {
        let query = "/api/events/search" + this.makeQueryString(filter);
        //execute http call
        return this.http.get(query)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    getEventsClustered(filter: Filter, clusteringFilter: ClusteringFilter): Observable<any> {
        let query = "api/events/search" + this.makeQueryString(filter, clusteringFilter);
        return this.http.get(query)
            .map((response: Response) => <MapPoint[]>response.json())
            .catch(this.handleError);
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

    updateRating(eventId: string, rating: number, currentrating: number, ratecount: number): Observable<Event> {
        return this.http.put("/api/events/update/rating?eventId=" + eventId + "&rating=" + rating + "&currentrating=" + currentrating + "&ratecount=" + ratecount, {})
            .map((response: Response) => {
                return <Event>response.json();
            }).catch(this.handleError);
    }

    updateReservation(eventId: string): Observable<Event> {
        return this.http.put("/api/events/update/reservation?eventId=" + eventId, {})
            .map((response: Response) => {
                return <Event>response.json();
            }).catch(this.handleError);
    }

    private makeQueryString(filter: Filter, clusteringFilter: ClusteringFilter = undefined): string {
        let query = "";

        query += "?category=";
        query += filter.Category.toString();

        if (filter.ULat != null && filter.ULong != null && filter.Radius != null && filter.Radius != 0) {
            query += "&uLat=" + filter.ULat.toString();
            query += "&uLong=" + filter.ULong.toString();
            query += "&radius=" + filter.Radius.toString();
        }

        if (filter.StartTime != null && filter.StartTime.toString() != "") {
            query += "&startTime=" + filter.StartTime.toString();
        }

        if (filter.EndTime != null && filter.EndTime.toString() != "") {
            query += "&endTime=" + filter.EndTime.toString();
        }

        if (filter.Price != null && filter.Price >= 0) {
            query += "&price=" + filter.Price.toString();
        }

        if (filter.RatingEvent != null && filter.RatingEvent >= 1 && filter.RatingEvent <= 5) {
            query += "&ratingEvent=" + filter.RatingEvent.toString();
        }

        if (filter.SearchString != null) {
            if (filter.SearchString.toString() != "") {
                query += "&searchString=" + filter.SearchString.toString();
            }
        }

        if (filter.Custom != null) {
            query += "&custom=" + filter.Custom;
        }

        if (!clusteringFilter) {
            query += "&pageNumber=" + filter.PageNumber.toString();
            query += "&pageSize=" + filter.PageSize.toString();
            query += "&orderAscending=" + filter.OrderIsAscending.toString();
            query += "&orderBy=" + filter.OrderByString.toString();
        }
        else {
            query += "&NELatitude=" + clusteringFilter.NELatitude.toString();
            query += "&NELongitude=" + clusteringFilter.NELongitude.toString();
            query += "&SWLatitude=" + clusteringFilter.SWLatitude.toString();
            query += "&SWLongitude=" + clusteringFilter.SWLongitude.toString();
            query += "&ZoomLevel=" + clusteringFilter.ZoomLevel.toString();
        }
        

        return query;
    }
}