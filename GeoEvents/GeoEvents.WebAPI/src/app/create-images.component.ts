import { Component } from '@angular/core'
import { Http, RequestOptions, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs/Rx'

@Component({
    templateUrl: "app/create-images.component.html"
})
export class CreateImagesComponent {
    private _formData: FormData

    constructor(private http: Http) { }

    get formData() {
        if (this._formData === undefined) {
            this._formData = new FormData();
        }
        return this._formData;
    }

    set formData(value: FormData) { this._formData = value; }

    upload(fileInput: any) {
        let fileList: FileList = fileInput.target.files;
        if (fileList.length > 0) {
            this.formData.append("<name>", fileList[0], fileList[0].name);

            let options = new RequestOptions();
            this.http.post('/api/images/create', this._formData, options)
                .map(res => res.json())
                .catch(error => Observable.throw(error))
                .subscribe(data => console.log(data), error => console.log(error))
        }
    }
}