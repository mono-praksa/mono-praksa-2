import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { Error404Component } from './components/error/error-404.component';
import { HomeComponent } from './components/home/home.component';

import { EventModule } from './components/event/event.module';
import { AppRoutingModule } from './app-routing.module';

import { PreserveSearchQuerryService } from './shared/preserve-search-querry.service';
import { LoaderService } from './shared/loader.service';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        RouterModule,
		EventModule,
		AppRoutingModule,
		FormsModule
    ],
    declarations: [
        AppComponent,
        NavbarComponent,
        Error404Component,
		HomeComponent
    ],
    bootstrap: [ AppComponent ],
    providers: [
        PreserveSearchQuerryService,
        LoaderService
    ]
	
})
export class AppModule { }
