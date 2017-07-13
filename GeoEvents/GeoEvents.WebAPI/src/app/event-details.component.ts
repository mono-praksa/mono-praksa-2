import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core'
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx'

import { IEvent } from './models/event.model'
import { IImage } from './models/image.model'

@Component({
    templateUrl: 'app/event-details.component.html',
    selector: 'event-details'
})

export class EventDetailsComponent implements OnInit{
    @Input() event: IEvent
    @Output() cancel = new EventEmitter()
    images: IImage[]

    constructor(private http: Http) {

    }

    ngOnInit() {
        this.getImages(this.event.Id).subscribe(res => {
            this.images = res
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