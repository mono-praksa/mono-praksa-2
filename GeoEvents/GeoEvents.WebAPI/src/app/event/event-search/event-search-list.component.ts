﻿import { Component, Input, OnChanges, OnInit } from "@angular/core";
import { MapsAPILoader } from "@agm/core";
import { Subscription } from "rxjs/Subscription";

import { Event } from "../shared/models/event.model";
import { EventService } from "../shared/event.service";
import { Filter } from "../shared/models/filter.model";
import { LoaderService } from "../../shared/loader.service";

@Component({
    selector: "display-list",
    templateUrl:"./event-search-list.component.html"
})

export class EventListComponent implements OnChanges, OnInit {
    @Input() events: Event[];
    @Input() filter: Filter;

    private dataServiceSubscription: Subscription;
    private errorMessage: string;
    private eventCount: number;
    private pageSize: number;
    private searchEventLoading: boolean = false;

    constructor(private eventService: EventService, private loaderService: LoaderService) { }

	//called on changes, gets the events from the database using the changed filter.
    ngOnChanges() {
        this.eventCount = undefined;
        this.getEvents(this.filter);
    }

	//clled on init. starts the service for displaying loading icons
    ngOnInit() {
        this.loaderService.loaderStatus.subscribe((value: boolean) => {
            this.searchEventLoading = value;
        });
    }

    //calls the http service and gets the events
    private getEvents(filter: Filter): void {
        this.loaderService.displayLoader(true);
        this.dataServiceSubscription = this.eventService.getEvents(filter)
            .subscribe(result => {
                this.events = result.data;
                this.eventCount = result.metaData.TotalItemCount;
                this.pageSize = result.metaData.PageSize;
                this.loaderService.displayLoader(false);
            }, error => this.errorMessage = <any>error);
    }

    //gets the events when user checks the ascending order checkbox
    private getEventsAscendingChanged() {
        if (this.filter.OrderIsAscending === undefined) {
            this.filter.OrderIsAscending = false;
        }
        else {
            this.filter.OrderIsAscending = !this.filter.OrderIsAscending;
        }
        //get the events
        this.getEvents(this.filter);
    }


    //gets the events when user changes the sorting order
    private getEventsOrderChanged(newOrder: string) {
        this.filter.OrderByString = newOrder;
        //get the events
        this.getEvents(this.filter);
    }

	//called on page change. changes the filter and gets the new page of events from the database
    private onPageChange(event: any) {
        this.filter.PageNumber = event.page + 1;
        this.getEvents(this.filter);
    }
}