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
    files: string[] = []
    private _formData: FormData;
    fileList: FileList;
    public uploading: boolean = false;
    public finished: boolean = false;
    public error: boolean = false;

    constructor(private http: Http) { }

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
        this.files = filesTmp.map((f: any) => f.name);
    }

    upload(fileInput: any) {
        this.finished = false;
        this.uploading = true;
        this.error = false;

        for (var i = 0; i < this.fileList.length; i++) {
            this.formData.append("name" + i, this.fileList[i], this.fileList[i].name);

            let options = new RequestOptions();
            this.http.post('/api/images/create', this.formData, options)
                .map(res => res.json())
                .catch(error => Observable.throw(error))
                .subscribe(data => {
                    console.log(data);
                    this.uploading = false;
                    this.finished = true;
                }, error => {
                    console.log(error);
                    this.error = true;
                });
            this.formData = new FormData();
        }
    }

    status(): string {
        if (this.error) {
            return "has-error";
        } else if (this.finished) {
            return "has-success";
        }
    }
}