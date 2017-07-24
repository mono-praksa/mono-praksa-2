import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Observable } from 'rxjs/Rx'

import { IEvent } from '../models/event.model'

import { LoaderService } from '../../../shared/loader.service'
import { EventService } from '../event.service'
import { LocationService } from '../location.service'

@Component({
    selector: 'create-customize',
    templateUrl: "app/components/event/views/event-create-customize.component.html"
})

export class EventCreateCustomizeComponent implements OnInit {
    key: FormControl;
    value: FormControl;
    customAttributeForm: FormGroup;

    @Input() createdEvent: IEvent;
    @Output() eventEmitter = new EventEmitter();
    private _createEventLoading: boolean = false;

    constructor(
        private _loaderService: LoaderService,
        private _eventService: EventService,
        private _locationService: LocationService
    ) { }

    ngOnInit() : void {
        this._loaderService.loaderStatus.subscribe((value: boolean) => {
            this.createEventLoading = value;
        });

        this.buildForm();
    }

    addCustomAttribute(formValues: any): void {
        if (!this.createdEvent.CustomModel) {
            this.createdEvent.CustomModel = [];
        }

        let i = this.createdEvent.CustomModel.findIndex(attr => { return attr.key === formValues.key});
        if (i !== -1) {
            this.createdEvent.CustomModel[i].values.push(formValues.value);
        }
        else {
            this.createdEvent.CustomModel.push({
                key: formValues.key,
                values: [formValues.value]
            });
        }
        this.resetValueControl();        
    }

    createEvent(): void {
        this.createdEvent.Custom = JSON.stringify(this.createdEvent.CustomModel);
        if (!this.createdEvent.Custom) {
            this.createdEvent.Custom = JSON.stringify("");
        }
        this._loaderService.displayLoader(true);
        this._eventService.createEvent(this.createdEvent).subscribe((response: IEvent) => {
            this.createdEvent = response;
            this.eventEmitter.emit(this.createdEvent);
            this._loaderService.displayLoader(false);
        });
    }

    private buildForm(): void {
        this.key = new FormControl('', Validators.required);
        this.value = new FormControl('', Validators.required);
        this.customAttributeForm = new FormGroup({
            key: this.key,
            value: this.value
        });
    }

    private resetValueControl(): void {
        this.customAttributeForm.controls["value"].setValue('');
        this.customAttributeForm.controls["value"].markAsUntouched();
    }

    private removeKey(keyIndex: number): void {
        this.createdEvent.CustomModel.splice(keyIndex, 1);
        if (this.createdEvent.CustomModel.length === 0) {
            this.createdEvent.CustomModel = undefined;
        }
    }

    private removeValue(keyIndex: number, valueIndex: number) {
        this.createdEvent.CustomModel[keyIndex].values.splice(valueIndex, 1);
        if (this.createdEvent.CustomModel[keyIndex].values.length === 0) {
            this.removeKey(keyIndex);
        }
    }

    get createEventLoading(): boolean {
        return this._createEventLoading;
    }

    set createEventLoading(isCreatingEvent: boolean) {
        this._createEventLoading = isCreatingEvent;
    }    
}