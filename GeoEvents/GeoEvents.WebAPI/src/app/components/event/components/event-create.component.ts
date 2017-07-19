import { Component } from '@angular/core'
import { IEvent } from '../models/event.model'

@Component({
    templateUrl: "app/components/event/views/event-create.component.html",
    styles: [`.oneThird {width: 50%;}
              .twoThirds {width: 75%;}`]
})
export class EventCreateComponent {
    createdEvent: IEvent;
    _skip: boolean = false;

    eventEmitted(event: IEvent) {
        this.createdEvent = event;
    }

    skip(variable: boolean) {
        this._skip = variable;
    }
}