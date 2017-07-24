import { Component, Input } from '@angular/core'
import { Http, RequestOptions, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import { IEvent } from '../models/event.model'
import { IImage } from '../models/image.model'
import { EventService } from '../event.service'

@Component({
    selector: "create-images",
    templateUrl: "app/components/event/views/event-create-images.component.html"
})
export class EventCreateImagesComponent {
    @Input() customizedEvent: IEvent;

    private _files: file[];
    private _uploadedFiles: string[] = [];
    private _formData: FormData;
    private _fileList: FileList;

    constructor(private _eventService: EventService) { }

    get files(): file[] {
        return this._files;
    }

    set files(theFiles: file[]) {
        this._files = theFiles;
    }

    get uploadedFiles(): string[] {
        return this._uploadedFiles;
    }

    set uploadedFiles(theFiles: string[]) {
        this._uploadedFiles = theFiles;
    }

    get fileList(): FileList {
        return this._fileList;
    }

    set fileList(theFileList: FileList) {
        this._fileList = theFileList;
    }

    get formData() {
        if (this._formData === undefined) {
            this._formData = new FormData();
        }
        return this._formData;
    }

    set formData(value: FormData) { this._formData = value; }

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

    upload() {
        this.files.forEach((listItem, i) => {
            this.files[i].uploading = true;
            this.formData.append("name" + i, this.fileList[i], this.fileList[i].name);

            this._eventService.createImage({
                Id: undefined,
                EventId: this.customizedEvent.Id,
                FormData: this.formData
            }).subscribe((data: any) => {
                this.files[i].uploading = false;
                this.files[i].finished = true;
                this.uploadedFiles.push(this.files[i].name);
                this.files.splice(i, 1);                
            }, (error: any) => {
                this.files[i].error = true;
            });
            this.formData = new FormData();
        })
    }
    uploadingFilter() {
        if (this.files) {
            return this.files.filter((file) => {
                return file.uploading;
            }).length > 0;
        }
        return false;
    }
}

interface file {
    name: string
    uploading: boolean
    finished: boolean
    error: boolean
}