"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var create_event_component_1 = require("./create-event.component");
var search_events_component_1 = require("./search-events.component");
var error_404_component_1 = require("./errors/error-404.component");
var create_images_component_1 = require("./create-images.component");
exports.routes = [
    { path: 'create/images', component: create_images_component_1.CreateImagesComponent },
    { path: 'create', component: create_event_component_1.CreateEventComponent },
    { path: 'search', component: search_events_component_1.SearchEventsComponent },
    { path: '', redirectTo: 'create', pathMatch: 'full' },
    { path: '404', component: error_404_component_1.Error404Component },
    { path: '**', redirectTo: '404' }
];
//# sourceMappingURL=routes.js.map