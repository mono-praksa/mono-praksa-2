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
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var core_2 = require("@agm/core");
var validators_1 = require("./validators");
var CreateEventComponent = (function () {
    function CreateEventComponent(http, formBuilder, mapsAPILoader, ngZone) {
        this.http = http;
        this.formBuilder = formBuilder;
        this.mapsAPILoader = mapsAPILoader;
        this.ngZone = ngZone;
        this.CategoryEnum = CategoryEnum;
        this.categories = [
            { id: CategoryEnum["Music"], checked: false },
            { id: CategoryEnum["Culture"], checked: false },
            { id: CategoryEnum["Sport"], checked: false },
            { id: CategoryEnum["Gastro"], checked: false },
            { id: CategoryEnum["Religious"], checked: false },
            { id: CategoryEnum["Business"], checked: false },
            { id: CategoryEnum["Miscellaneous"], checked: true }
        ];
    }
    CreateEventComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.name = new forms_1.FormControl('', forms_1.Validators.required);
        this.description = new forms_1.FormControl('', forms_1.Validators.required);
        this.start = new forms_1.FormControl('', forms_1.Validators.required);
        this.end = new forms_1.FormControl('', forms_1.Validators.required);
        this.eventForm = this.formBuilder.group({
            name: this.name,
            description: this.description,
            start: this.start,
            end: this.end
        }, { validator: validators_1.endDateBeforeStartDate('start', 'end') });
        //GOOGLE MAPS
        this.zoom = 4;
        this.latitude = 39.8282;
        this.longitude = -98.5795;
        this.setCurrentPosition();
        this.mapsAPILoader.load().then(function () {
            var autocomplete = new google.maps.places.Autocomplete(_this.searchElementRef.nativeElement, {
                types: ["address"]
            });
            autocomplete.addListener("place_changed", function () {
                _this.ngZone.run(function () {
                    //get the place result
                    var place = autocomplete.getPlace();
                    //verify result
                    if (place.geometry === undefined || place.geometry === null) {
                        return;
                    }
                    //set latitude, longitude and zoom
                    _this.latitude = place.geometry.location.lat();
                    _this.longitude = place.geometry.location.lng();
                    _this.zoom = 12;
                });
            });
        });
    };
    CreateEventComponent.prototype.setCurrentPosition = function () {
        var _this = this;
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition(function (position) {
                _this.latitude = position.coords.latitude;
                _this.longitude = position.coords.longitude;
                _this.zoom = 12;
            });
        }
    };
    CreateEventComponent.prototype.handleError = function (error) {
        return Rx_1.Observable.throw(error.statusText);
    };
    CreateEventComponent.prototype.createEvent = function (formValues) {
        var chosenCategories = [];
        this.categories.filter(function (checkbox) {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });
        var newEvent = {
            Name: formValues.name,
            Description: formValues.description,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Lat: this.latitude,
            Long: this.longitude,
            Categories: chosenCategories
        };
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        var options = new http_1.RequestOptions({ headers: headers });
        return this.http.post('/api/event/create', JSON.stringify(newEvent), options).map(function (response) {
            return response.json();
        }).catch(this.handleError).subscribe(function (response) {
            console.log(response);
        });
    };
    CreateEventComponent.prototype.updateCategories = function (category) {
        this.categories.filter(function (checkbox) {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    };
    CreateEventComponent.prototype.mapClicked = function ($event) {
        this.latitude = $event.coords.lat;
        this.longitude = $event.coords.lng;
    };
    CreateEventComponent.prototype.isAllUnchecked = function () {
        var checkbox;
        for (var _i = 0, _a = this.categories; _i < _a.length; _i++) {
            checkbox = _a[_i];
            if (checkbox.checked) {
                return false;
            }
        }
        return true;
    };
    CreateEventComponent.prototype.keys = function () {
        var keys = Object.keys(CategoryEnum);
        return keys.slice(keys.length / 2);
    };
    return CreateEventComponent;
}());
__decorate([
    core_1.ViewChild("search"),
    __metadata("design:type", core_1.ElementRef)
], CreateEventComponent.prototype, "searchElementRef", void 0);
CreateEventComponent = __decorate([
    core_1.Component({
        templateUrl: "app/create-event.component.html",
        styles: ["\n        agm-map {\n            height: 300px;\n        }\n    "]
    }),
    __metadata("design:paramtypes", [http_1.Http, forms_1.FormBuilder, core_2.MapsAPILoader, core_1.NgZone])
], CreateEventComponent);
exports.CreateEventComponent = CreateEventComponent;
var CategoryEnum;
(function (CategoryEnum) {
    CategoryEnum[CategoryEnum["Music"] = 1] = "Music";
    CategoryEnum[CategoryEnum["Culture"] = 2] = "Culture";
    CategoryEnum[CategoryEnum["Sport"] = 4] = "Sport";
    CategoryEnum[CategoryEnum["Gastro"] = 8] = "Gastro";
    CategoryEnum[CategoryEnum["Religious"] = 16] = "Religious";
    CategoryEnum[CategoryEnum["Business"] = 32] = "Business";
    CategoryEnum[CategoryEnum["Miscellaneous"] = 64] = "Miscellaneous";
})(CategoryEnum = exports.CategoryEnum || (exports.CategoryEnum = {}));
//# sourceMappingURL=create-event.component.js.map