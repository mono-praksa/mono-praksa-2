import { Component, Input } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { Event } from "../../shared/models/event.model";
import { EventService } from "../../shared/event.service";
import { File } from "../../shared/models/file.model";

@Component({
    selector: "create-images",
    templateUrl: "app/event/event-create/event-create-images/event-create-images.component.html"
})
export class EventCreateImagesComponent {
    @Input() customizedEvent: Event;
    
    private fileList: FileList;
    private files: File[];
    private formData: FormData = new FormData();
    private uploadedFiles: string[] = [];

    constructor(private eventService: EventService) { }

	//called on changes, removes the uploaded files from the filelist and adds their names to the uploaded files list
    onChange(fileInput: any) {
        this.fileList = fileInput.target.files;
        let filesTmp = [].slice.call(this.fileList);

        this.files = [];

        for (var el of filesTmp) {
            this.files.push({
                name: el.name,
                uploading: false,
                finished: false,
                error: false
            })
        }
    }

	//for each file calls the service and posts the file to the server
    upload() {
        let counter = 0;
        this.files.forEach((listItem, i) => {
            this.files[i].uploading = true;
            this.formData.append("name" + i, this.fileList[i], this.fileList[i].name);

            this.eventService.createImage({
                Id: undefined,
                EventId: this.customizedEvent.Id,
                FormData: this.formData
            }).subscribe((data: any) => {
                counter++;
                this.files[i].uploading = false;
                this.files[i].finished = true;

                this.uploadedFiles.push(this.files[i].name);
                if (counter == this.files.length) {
                    this.files = [];
                }             
            }, (error: any) => {
                this.files[i].error = true;
            });
            this.formData = new FormData();
        })
    }

	//checks wehter the files are uploading and sets the filter accordingly
    uploadingFilter() {
        if (this.files) {
            return this.files.filter((file) => {
                return file.uploading;
            }).length > 0;
        }
        return false;
    }
}