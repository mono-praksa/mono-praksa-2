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

    private _files: Array<file>;
    private _indices: Array<number>;
    private _formData: FormData;
    private _fileList: FileList;
    private _btnUploadClicked: boolean = false;
    @Output() emittSkip = new EventEmitter();

    constructor(private http: Http) { }

    get files(): Array<file> {
        return this._files;
    }

    set files(theFiles: Array<file>) {
        this._files = theFiles;
    }

    get indices(): Array<number> {
        return this._indices;
    }

    set indices(theIndices: Array<number>) {
        this._indices = theIndices;
    }

    get fileList(): FileList {
        return this._fileList;
    }

    set fileList(theFileList: FileList) {
        this._fileList = theFileList;
    }

    get btnUploadClicked(): boolean {
        return this._btnUploadClicked;
    }

    set bntUploadClicked(isBtnUploadClicked: boolean) {
        this._btnUploadClicked = isBtnUploadClicked;
    }

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