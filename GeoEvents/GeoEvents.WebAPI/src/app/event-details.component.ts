import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'

import { IEvent } from './models/event.model'
import { IImage } from './models/image.model'

@Component({
    templateUrl: 'app/event-details.component.html',
    selector: 'event-details',
    styles: [`
    .carousel {
        width: 640px;
        max-width: 640px;
        height: 480px;
        max-height: 480px;
}

    .carousel img {
        width: 640px;
        max-width: 640px;
        height: 480px;
        max-height: 480px;
}

    .eventName {
        float: left;
}

    .eventTime {
        float: right;
}
`]
})

export class EventDetailsComponent implements OnInit{
    @Input() event: IEvent
    @Output() cancel = new EventEmitter()
    images: IImage[]
    CategoryEnum: any = CategoryEnum
    imagesLoading: boolean = true
    address: string = ""

    constructor(private http: Http) {

    }

    ngOnInit() {
        this.getImages(this.event.Id).subscribe(res => {
            this.imagesLoading = false
            this.images = res
        })

        this.http.get('https://maps.googleapis.com/maps/api/geocode/json?latlng=' + this.event.Lat + ',' + this.event.Long + '&key=AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w')
            .map((response: Response) => {
                return response;
            }).catch(this.handleError).subscribe(response => {
                console.log(response.json())
                this.address = response.json()["results"][0]["formatted_address"];
                console.log(this.address)
            })
    }

    getImages(id : string): Observable<IImage[]> {
        return this.http.get('/api/images/get/' + this.event.Id).map(function (response: Response) {
            return <IImage[]>response.json();
        }).catch(this.handleError);
    }

    handleError(error: Response) {
        return Observable.throw(error.statusText);
    }

    handleCancelClick() {
        this.cancel.emit()
    }

}

enum CategoryEnum {
    Music = 1,
    Culture = 2,
    Sport = 4,
    Gastro = 8,
    Religious = 16,
    Business = 32,
    Miscellaneous = 64
}