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
var SearchEventsComponent = (function () {
    function SearchEventsComponent(http, formBuilder, mapsAPILoader, ngZone) {
        this.http = http;
        this.formBuilder = formBuilder;
        this.mapsAPILoader = mapsAPILoader;
        this.ngZone = ngZone;
        this.categories = [
            { id: 1, checked: false },
            { id: 2, checked: false },
            { id: 4, checked: false },
            { id: 8, checked: false },
            { id: 16, checked: false },
            { id: 32, checked: false },
            { id: 64, checked: false }
        ];
        this.mapMode = false;
        this.detailsMode = false;
    }
    SearchEventsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.start = new forms_1.FormControl('');
        this.end = new forms_1.FormControl('');
        this.address = new forms_1.FormControl('');
        this.radius = new forms_1.FormControl('');
        this.filterForm = this.formBuilder.group({
            start: this.start,
            end: this.end,
            address: this.address,
            radius: this.radius
        });
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
                });
            });
        });
    };
    SearchEventsComponent.prototype.setCurrentPosition = function () {
        var _this = this;
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition(function (position) {
                _this.latitude = position.coords.latitude;
                _this.longitude = position.coords.longitude;
            });
        }
    };
    SearchEventsComponent.prototype.updateCategories = function (category) {
        this.categories.filter(function (checkbox) {
            if (checkbox.id == category) {
                checkbox.checked = !checkbox.checked;
            }
        });
    };
    SearchEventsComponent.prototype.changeDisplayMode = function () {
        this.mapMode = !this.mapMode;
    };
    SearchEventsComponent.prototype.eventDetails = function (event) {
        this.event = event;
        this.detailsMode = true;
    };
    SearchEventsComponent.prototype.filterEvents = function (formValues) {
        var _this = this;
        var chosenCategories = [];
        this.categories.filter(function (checkbox) {
            if (checkbox.checked) {
                chosenCategories.push(checkbox.id);
            }
        });
        var cat = 0;
        for (var _i = 0, chosenCategories_1 = chosenCategories; _i < chosenCategories_1.length; _i++) {
            var c = chosenCategories_1[_i];
            cat += c;
        }
        var filter = {
            ULat: this.latitude,
            ULong: this.longitude,
            Radius: formValues.radius,
            StartTime: formValues.start,
            EndTime: formValues.end,
            Category: cat
        };
        console.log('form values: ', formValues);
        console.log('filter: ', filter);
        this.getEvents(filter).subscribe(function (res) {
            _this.events = res;
        });
    };
    SearchEventsComponent.prototype.getEvents = function (filter) {
        return this.http.get('/api/event/search/' + filter.ULat.toString() + '/' + filter.ULong.toString() + '/' + filter.Radius.toString() + '/' + filter.Category.toString() + '/' + filter.StartTime.toString().replace(':', 'h') + '/' + filter.EndTime.toString().replace(':', 'h')).map(function (response) {
            return response.json();
        }).catch(this.handleError);
    };
    SearchEventsComponent.prototype.handleError = function (error) {
        return Rx_1.Observable.throw(error.statusText);
    };
    return SearchEventsComponent;
}());
__decorate([
    core_1.ViewChild("search"),
    __metadata("design:type", core_1.ElementRef)
], SearchEventsComponent.prototype, "searchElementRef", void 0);
SearchEventsComponent = __decorate([
    core_1.Component({
        templateUrl: "app/search-events.component.html",
        styles: ["/* The switch - the box around the slider */\n.switch {\n  position: relative;\n  display: inline-block;\n  width: 60px;\n  height: 34px;\n}\n\n/* Hide default HTML checkbox */\n.switch input {display:none;}\n\n/* The slider */\n.slider {\n  position: absolute;\n  cursor: pointer;\n  top: 0;\n  left: 0;\n  right: 0;\n  bottom: 0;\n  background-color: #ccc;\n  -webkit-transition: .4s;\n  transition: .4s;\n}\n\n.slider:before {\n  position: absolute;\n  content: \"\";\n  height: 26px;\n  width: 26px;\n  left: 4px;\n  bottom: 4px;\n  background-color: white;\n  -webkit-transition: .4s;\n  transition: .4s;\n}\n\ninput:checked + .slider {\n  background-color: #2196F3;\n}\n\ninput:focus + .slider {\n  box-shadow: 0 0 1px #2196F3;\n}\n\ninput:checked + .slider:before {\n  -webkit-transform: translateX(26px);\n  -ms-transform: translateX(26px);\n  transform: translateX(26px);\n}"]
    }),
    __metadata("design:paramtypes", [http_1.Http, forms_1.FormBuilder, core_2.MapsAPILoader, core_1.NgZone])
], SearchEventsComponent);
exports.SearchEventsComponent = SearchEventsComponent;
//# sourceMappingURL=search-events.component.js.map