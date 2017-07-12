import { Component, Input } from '@angular/core'
import { Http, RequestOptions, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs/Rx'
import { IEvent } from './../models/event.model'

@Component({
    selector: "create-images",
    templateUrl: "app/create/create-images.component.html"
})
export class CreateImagesComponent {
    @Input() createdEvent: IEvent;

    files: Array<string> = [];
    indices: Array<number> = [];
    private _formData: FormData;
    fileList: FileList;

    public uploading: boolean[] = [];
    public finished: Array<boolean> = [];
    public error: Array<boolean> = [];

    constructor(private http: Http) { }

    get formData() {
        if (this._formData === undefined) {
            this._formData = new FormData();
        }
        return this._formData;
    }

    set formData(value: FormData) { this._formData = value; }

    onChange(fileInput: any) {
        let i: number = 0;

        this.fileList = fileInput.target.files;
        let filesTmp = [].slice.call(this.fileList);
        //this.files = filesTmp.map((f: any) => f.name);

        for (var el of filesTmp) {
            this.files.push(el.name);
            this.indices.push(i);
            this.finished.push(false);
            this.uploading.push(false);
            this.error.push(false);
            i += 1;
        }
    }

    upload(fileInput: any) {
        for (var i = 0; i < this.fileList.length; i++) {
            this.uploading[i] = true;
            this.formData.append("name" + i, this.fileList[i], this.fileList[i].name);

            let options = new RequestOptions();
            this.http.post('/api/images/create', this.formData, options)
                .map(res => res.json())
                .catch(error => Observable.throw(error))
                .subscribe(data => {
                    console.log(data);
                    this.uploading[i] = false;
                    this.finished[i] = true;
                }, error => {
                    console.log(error);
                    this.error[i] = true;
                });
            this.formData = new FormData();
        }
    }
    uploadingFilter() {
        if (this.uploading.length != 0) {
            return this.uploading.filter(f => { if (f) return f }).length != 0;
        }
        return false;
    }
}