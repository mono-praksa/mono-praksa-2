import { Component } from '@angular/core'
import { IEvent } from '../models/event.model'

@Component({
    templateUrl: "app/components/event/views/event-create.component.html",
    styles: [`.stepOne {width: 50%;}
              .stepTwo {width: 75%;}
              .stepThree {width: 100%;`]
})
export class EventCreateComponent {
    createdEvent: IEvent;
    customizedEvent: IEvent;
    _skip: boolean = false;

    createEvent(event: IEvent) {
        this.createdEvent = event;
    }

    customizeEvent(event: IEvent) {
        this.customizedEvent = event;
    }

    skip(variable: boolean) {
        this._skip = variable;
    }
}