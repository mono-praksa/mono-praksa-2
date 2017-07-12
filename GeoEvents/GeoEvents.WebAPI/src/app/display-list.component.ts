﻿import { Component, Output, EventEmitter, Input } from '@angular/core'
import { IEvent } from './event.model'

@Component({
    templateUrl:'app/display-list.component.html',
    selector: 'display-list'
})

export class DisplayListComponent {
    @Input() events : IEvent[]
    @Output() event = new EventEmitter()

    eventDetails(evt: IEvent) {
        this.event.emit(evt);
    }

}