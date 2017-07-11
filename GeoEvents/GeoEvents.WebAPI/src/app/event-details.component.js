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
var EventDetailsComponent = (function () {
    function EventDetailsComponent(http) {
        this.http = http;
    }
    EventDetailsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.getImages(this.event.Id).subscribe(function (res) {
            _this.images = res;
        });
    };
    EventDetailsComponent.prototype.getImages = function (id) {
        return this.http.get('/api/images/get/' + this.event.Id).map(function (response) {
            return response.json();
        }).catch(this.handleError);
    };
    EventDetailsComponent.prototype.handleError = function (error) {
        return Rx_1.Observable.throw(error.statusText);
    };
    return EventDetailsComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], EventDetailsComponent.prototype, "event", void 0);
EventDetailsComponent = __decorate([
    core_1.Component({
        templateUrl: 'app/event-details.component.html',
        selector: 'event-details'
    }),
    __metadata("design:paramtypes", [http_1.Http])
], EventDetailsComponent);
exports.EventDetailsComponent = EventDetailsComponent;
//# sourceMappingURL=event-details.component.js.map