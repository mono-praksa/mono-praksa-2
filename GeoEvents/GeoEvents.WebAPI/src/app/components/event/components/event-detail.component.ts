import { Component, Input, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
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

    .rating {
        float:left;
    }

    .rating:not(:checked) > input {
        position:absolute;
        top:-9999px;
        clip:rect(0,0,0,0);
    }

    .rating:not(:checked) > label {
        float:right;
        width:1em;
        padding:0.1em;
        overflow:hidden;
        white-space:nowrap;
        cursor:pointer;
        font-size:200%;
        line-height:1.2;
        color:#ddd;
    }

    .rating:not(:checked) > label:before {
        content: '★ ';
    }

    .rating > input:checked ~ label {
        color: #f70;
    }

    .rating:not(:checked) > label:hover,
    .rating:not(:checked) > label:hover ~ label {
        color: gold;
    }


    .rating > input:checked ~ label:hover,
    .rating > input:checked ~ label:hover ~ label,
    .rating > label:hover ~ input:checked ~ label {
        color: #ea0;
    }
`]
})

export class EventDetailComponent implements OnInit{
    //@Input() event: IEvent;
    private _event: IEvent;
    @ViewChild("carousel") carouselElement: ElementRef;
    @ViewChild("userRate") userRateElement: ElementRef;
    private _images: IImage[];
    CategoryEnum: any = CategoryEnum;
    private _address: string = "";
    private _getImagesLoading: boolean = false;

    @Input() rating: number;
    @Input() itemId: number;
    @Output() ratingClick: EventEmitter<any> = new EventEmitter<any>();

    inpustName: string;

    constructor(
        private geocodingService: GeocodingService,
        private eventService: EventService,
        private _loaderService: LoaderService,
        private activatedRoute: ActivatedRoute
    ) {

    }

    ngOnInit() {
        this.inpustName = this.itemId + '_rating';
        this.eventService.getEventById(this.activatedRoute.snapshot.params['eventId'])
            .subscribe((event: IEvent) => { this.event = event; console.log(event) });
        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.getImagesLoading = value;
        });

        this._loaderService.displayLoader(true);

        this.eventService.getImages(this.activatedRoute.snapshot.params['eventId']).subscribe((res: IImage[]) => {
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

        //this.geocodingService.getAddress(this.event.Lat, this.event.Long).subscribe(response => {
        //    this.address = response;
        //});
    }

    rateChange(slider: any) {
        this.userRateElement.nativeElement.innerHTML = slider.value;
    }

    rate(rating: number) {
        this.eventService.updateRating(this.event.Id, +rating)
            .subscribe((response: IEvent) => {
                this.event.Rating = response.Rating;
                this.event.RateCount = response.RateCount;
                this.rating = rating;
                this.ratingClick.emit({
                    itemId: this.itemId,
                    rating: rating
                });
            });

    }

    reserve() {
        this.eventService.updateReservation(this.event.Id)
            .subscribe((response: IEvent) => {
                this.event.Reserved = response.Reserved;
            });
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
    }

    get event(): IEvent {
        return this._event;
    }

    set event(thisEvent: IEvent) {
        this._event = thisEvent;
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

    //onClick(rating: number): void {
    //    this.rating = rating;
    //    this.ratingClick.emit({
    //        itemId: this.itemId,
    //        rating: rating
    //    });
    //}
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