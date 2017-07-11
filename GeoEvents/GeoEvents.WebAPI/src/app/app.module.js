"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var router_1 = require("@angular/router");
var datetime_picker_1 = require("@ngui/datetime-picker");
var core_2 = require("@agm/core");
var app_component_1 = require("./app.component");
var create_event_component_1 = require("./create-event.component");
var search_events_component_1 = require("./search-events.component");
var display_list_component_1 = require("./display-list.component");
var display_map_component_1 = require("./display-map.component");
var event_details_component_1 = require("./event-details.component");
var navbar_component_1 = require("./navbar/navbar.component");
var error_404_component_1 = require("./errors/error-404.component");
var create_images_component_1 = require("./create-images.component");
var routes_1 = require("./routes");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            forms_1.ReactiveFormsModule,
            http_1.HttpModule,
            router_1.RouterModule.forRoot(routes_1.routes),
            datetime_picker_1.NguiDatetimePickerModule,
            core_2.AgmCoreModule.forRoot({
                apiKey: 'AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w',
                libraries: ['places']
            })
        ],
        declarations: [
            app_component_1.AppComponent,
            create_event_component_1.CreateEventComponent,
            search_events_component_1.SearchEventsComponent,
            display_list_component_1.DisplayListComponent,
            display_map_component_1.DisplayMapComponent,
            event_details_component_1.EventDetailsComponent,
            navbar_component_1.NavbarComponent,
            error_404_component_1.Error404Component,
            create_images_component_1.CreateImagesComponent
        ],
        bootstrap: [
            app_component_1.AppComponent
        ]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map