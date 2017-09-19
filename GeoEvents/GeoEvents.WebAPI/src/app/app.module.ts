import { NgModule } from "@angular/core";
import { HttpModule } from "@angular/http";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AppComponent } from "./app.component";
import { AppRoutingModule } from "./app-routing.module";
import { EventModule } from "./event/event.module";
import { Error404Component } from "./error/error-404.component";
import { HomeComponent } from "./home/home.component";
import { LoaderService } from "./shared/loader.service";
import { NavbarComponent } from "./shared/navbar/navbar.component";

@NgModule({
    imports: [
		EventModule,
		AppRoutingModule,
        BrowserModule,
        FormsModule,
        HttpModule,
        ReactiveFormsModule,
        RouterModule
    ],
    declarations: [
        AppComponent,
        Error404Component,
		HomeComponent,
        NavbarComponent
    ],
    bootstrap: [ AppComponent ],
    providers: [
        LoaderService
    ]
	
})
export class AppModule { }
