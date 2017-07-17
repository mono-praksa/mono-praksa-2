import { Component, Input, EventEmitter, Output } from '@angular/core'
import { Http, RequestOptions, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import { IEvent } from '../models/event.model'


@Component({
    selector: "create-images",
    templateUrl: "app/components/event/views/event-create-images.component.html"
})
export class EventCreateImagesComponent {
    @Input() createdEvent: IEvent;

    files: Array<file>;
    indices: Array<number>;
    private _formData: FormData;
    fileList: FileList;
    btnUploadClicked: boolean = false;
    @Output() emittSkip = new EventEmitter();

    constructor(private http: Http) { }

    get formData() {
        if (this._formData === undefined) {
            this._formData = new FormData();
        }
        return this._formData;
    }

    set formData(value: FormData) { this._formData = value; }

    onChange(fileInput: any, showFileNum: any) {
        let i: number = 0;

        this.fileList = fileInput.target.files;
        let filesTmp = [].slice.call(this.fileList);

        this.files = [];
        this.indices = [];

        for (var el of filesTmp) {
            this.files.push({
                name: el.name,
                uploading: false,
                finished: false,
                error: false
            })
            this.indices.push(i);
            i += 1;
        }
        showFileNum.value = i + " files selected";
    }

    upload(fileInput: any) {
        this.btnUploadClicked = true;
        this.files.forEach((listItem, i) => {
            this.files[i].uploading = true;
            this.formData.append("name" + i, this.fileList[i], this.fileList[i].name);

            let options = new RequestOptions();
            this.http.post('/api/images/create/' + this.createdEvent.Id, this.formData, options)
                .map((res: Response) => res.json())
                .catch((error:any) => Observable.throw(error))
                .subscribe((data:any) => {
                    this.files[i].uploading = false;
                    this.files[i].finished = true;
                }, (error:any) => {
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

    skip() {
        this.emittSkip.emit(true);
    }
}

interface file {
    name: string
    uploading: boolean
    finished: boolean
    error: boolean
}