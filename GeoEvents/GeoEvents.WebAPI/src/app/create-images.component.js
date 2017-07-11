"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var CreateImagesComponent = (function () {
    function CreateImagesComponent(http) {
        this.http = http;
    }
    Object.defineProperty(CreateImagesComponent.prototype, "formData", {
        get: function () {
            if (this._formData === undefined) {
                this._formData = new FormData();
            }
            return this._formData;
        },
        set: function (value) { this._formData = value; },
        enumerable: true,
        configurable: true
    });
    CreateImagesComponent.prototype.upload = function (fileInput) {
        var fileList = fileInput.target.files;
        if (fileList.length > 0) {
            this.formData.append("<name>", fileList[0], fileList[0].name);
            var options = new http_1.RequestOptions();
            this.http.post('/api/images/create', this._formData, options)
                .map(function (res) { return res.json(); })
                .catch(function (error) { return Rx_1.Observable.throw(error); })
                .subscribe(function (data) { return console.log(data); }, function (error) { return console.log(error); });
        }
    };
    return CreateImagesComponent;
}());
CreateImagesComponent = __decorate([
    core_1.Component({
        templateUrl: "app/create-images.component.html"
    }),
    __metadata("design:paramtypes", [http_1.Http])
], CreateImagesComponent);
exports.CreateImagesComponent = CreateImagesComponent;
//# sourceMappingURL=create-images.component.js.map