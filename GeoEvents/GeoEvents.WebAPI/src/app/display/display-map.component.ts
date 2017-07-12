import { Component, Output, EventEmitter, Input } from '@angular/core'
import { IEvent } from './../models/event.model'

@Component({
    template: '<h1>I AM DISPLAY MAP COMPONENT</h1>',
    selector: 'display-map',
    styles: [`
        agm-map {
            height: 300px;
        }
    `]
})

export class DisplayMapComponent {
    @Input() events: IEvent[]
    @Output() event = new EventEmitter()

    displayEvent(evt: IEvent) {
        this.event.emit(evt);
    }
}