import { Component, Input, OnInit, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { IEvent } from '../models/event.model';
import { IImage } from '../models/image.model';

import { GeocodingService } from '../../../shared/geocoding.service';
import { EventService } from '../event.service';
import { LoaderService } from '../../../shared/loader.service';

@Component({
    templateUrl: 'app/components/event/views/event-detail.component.html',
    selector: 'event-details',
    styles: [`
    .carousel, :host /deep/ img, :host /deep/ svg {
        width: 640px;
        max-width: 640px;
        height: 480px;
        max-height: 480px;
}
`]
})

export class EventDetailComponent implements OnInit{
    @Input() event: IEvent;
    @Output() cancel = new EventEmitter();
    @ViewChild("carousel") carouselElement: ElementRef;
    @ViewChild("userRate") userRateElement: ElementRef;
    private _images: IImage[];
    CategoryEnum: any = CategoryEnum;
    private _address: string = "";
    private _getImagesLoading: boolean = false;

    constructor(
        private geocodingService: GeocodingService,
        private eventService: EventService,
        private _loaderService: LoaderService
    ) {

    }

    ngOnInit() {
        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.getImagesLoading = value;
        });

        this._loaderService.displayLoader(true);

        this.eventService.getImages(this.event.Id).subscribe((res: IImage[]) => {
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

                this.carouselElement.nativeElement.appendChild(item);
            }
            this._loaderService.displayLoader(false);
        });

        this.geocodingService.getAddress(this.event.Lat, this.event.Long).subscribe(response => {
            this.address = response;
        });
    }

    rateChange(slider: any) {
        console.log(slider.nativeElement);
        this.userRateElement.nativeElement.innerHTML = slider.value;
    }

    rate() {
        let rating = this.userRateElement.nativeElement.innerHTML;
        this.eventService.updateRating(this.event.Id, +rating)
            .subscribe(response => console.log(response));
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

    get getImagesLoading(): boolean {
        return this._getImagesLoading;
    }

    set getImagesLoading(thisGetImagesLoading: boolean) {
        this._getImagesLoading = thisGetImagesLoading;
    }

    get images() : IImage[] {
        return this._images;
    }

    set images(theImages: IImage[]) {
        this._images = theImages;
    }

    get address(): string {
        return this._address;
    }

    set address(theAddress: string) {
        this._address = theAddress;
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