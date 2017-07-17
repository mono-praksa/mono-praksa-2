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

export class EventDetailsComponent implements OnInit {
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
            for (var i = 0; i < this.images.length; i++) {
                var item = document.createElement("div");

                if (i == 0) item.setAttribute("class", "item active");
                else item.setAttribute("class", "item");

                if (this.images[i].Content.substr(0, 10) != 'PD94bWwgdm') {
                    var img = document.createElement("img");
                    img.setAttribute("src", "data:image/jpeg;base64," + this.images[i].Content);

                    item.appendChild(img);
                } else {
                    var svg = this.parseSvg(this.decodeBase64(this.images[i].Content));

                    item.appendChild(svg);
                }

                document.getElementById("carousel-inner").appendChild(item);
            }
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

    getImages(id: string): Observable<IImage[]> {
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

    decodeBase64(s: string) {
        var e = {}, i, b = 0, c, x, l = 0, a, r = '', w = String.fromCharCode, L = s.length;
        var A = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        for (i = 0; i < 64; i++) { e[A.charAt(i)] = i; }
        for (x = 0; x < L; x++) {
            c = e[s.charAt(x)]; b = (b << 6) + c; l += 6;
            while (l >= 8) { ((a = (b >>> (l -= 8)) & 0xff) || (x < (L - 2))) && (r += w(a)); }
        }
        return r;
    }

    parseSvg(xmlString: string) {
        let parser = new DOMParser();
        let doc = parser.parseFromString(xmlString, "image/svg+xml");
        return doc.documentElement;
        //document.getElementById("carousel-inner").appendChild(doc.documentElement);
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