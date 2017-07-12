import { Component, Output, EventEmitter, Input } from '@angular/core'
import { IEvent } from './../models/event.model'
import { MapsAPILoader } from '@agm/core'

@Component({
    selector: 'display-map',
    styles: [`
    agm-map {
      height: 500px;
    }
  `],
    template: `
  <agm-map [latitude]="lat" [longitude]="lng"></agm-map>
  `
})

export class DisplayMapComponent {
    @Input() events: IEvent[]
    lat: number = 51.678418;
    lng: number = 7.809007;
    @Output() event = new EventEmitter()

    displayEvent(evt: IEvent) {
        this.event.emit(evt);
    }
}