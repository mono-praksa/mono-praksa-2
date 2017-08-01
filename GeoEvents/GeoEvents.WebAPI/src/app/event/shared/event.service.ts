import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/throw";
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
        let query = "/api/events/clustered" + this.makeQueryString(filter, clusteringFilter);
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
        let queryParamChar = "?";
        if (filter.Category && filter.Category >= 0) {
            query += queryParamChar + "category=" + filter.Category.toString();
            queryParamChar = "&";
        }

        if (!(filter.ULat === undefined || filter.ULat === null)
            && !(filter.ULong === undefined || filter.ULong === null)
            && !(filter.Radius === undefined || filter.Radius === null || filter.Radius === 0))
        {
            query += queryParamChar + "uLat=" + filter.ULat.toString();
            queryParamChar = "&";
            query += queryParamChar + "uLong=" + filter.ULong.toString();
            query += queryParamChar + "radius=" + filter.Radius.toString();
        }

        if (!(filter.StartTime === undefined || filter.StartTime === null || filter.StartTime.toString() === "")) {
            query += queryParamChar + "startTime=" + filter.StartTime.toString();
            queryParamChar = "&";
        }

        if (!(filter.EndTime === undefined || filter.EndTime === null || filter.EndTime.toString() === "")) {
            query += queryParamChar + "endTime=" + filter.EndTime.toString();
            queryParamChar = "&";
        }

        if (!(filter.Price === undefined || filter.Price === null || filter.Price.toString() === "")) {
            query += queryParamChar + "price=" + filter.Price.toString();
            queryParamChar = "&";
        }

        if (!(filter.RatingEvent === undefined || filter.RatingEvent === null || filter.RatingEvent.toString() === "")) {
            query += queryParamChar + "ratingEvent=" + filter.RatingEvent.toString();
            queryParamChar = "&";
        }

        if (!(filter.SearchString === undefined || filter.SearchString === null || filter.SearchString.toString() === "")) {
            query += queryParamChar + "searchString=" + filter.SearchString.toString();
            queryParamChar = "&";
        }

        if (!(filter.Custom === undefined || filter.Custom === null || filter.Custom.toString() === "")) {
            query += queryParamChar + "custom=" + filter.Custom;
            queryParamChar = "&";
        }

        if (!clusteringFilter) {
            query += queryParamChar + "pageNumber=" + filter.PageNumber.toString();
            queryParamChar = "&";
            if (!(filter.OrderIsAscending === undefined || filter.OrderIsAscending === null))
            {
                query += queryParamChar + "orderAscending=" + filter.OrderIsAscending.toString();
            }
            if (!(filter.OrderByString === undefined || filter.OrderByString === null || filter.OrderByString.toString() === "")) {
                query += queryParamChar + "orderBy=" + filter.OrderByString.toString();
            }
        }
        else if (clusteringFilter.ZoomLevel) {
            query += queryParamChar + "NELatitude=" + clusteringFilter.NELatitude.toString();
            queryParamChar = "&";
            query += queryParamChar + "NELongitude=" + clusteringFilter.NELongitude.toString();
            query += queryParamChar + "SWLatitude=" + clusteringFilter.SWLatitude.toString();
            query += queryParamChar + "SWLongitude=" + clusteringFilter.SWLongitude.toString();
            query += queryParamChar + "ZoomLevel=" + clusteringFilter.ZoomLevel.toString();
        }
        

        return query;
    }
}