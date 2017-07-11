import { NgModule }      from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { ReactiveFormsModule } from '@angular/forms'
import { HttpModule } from '@angular/http'
import { RouterModule } from '@angular/router'
import { NguiDatetimePickerModule } from '@ngui/datetime-picker'
import { AgmCoreModule } from '@agm/core'

import { AppComponent } from './app.component'
import { CreateEventComponent } from './create-event.component'
import { SearchEventsComponent } from './search-events.component'
import { DisplayListComponent } from './display-list.component'
import { DisplayMapComponent } from './display-map.component'
import { EventDetailsComponent } from './event-details.component'
import { NavbarComponent } from './navbar/navbar.component'
import { Error404Component } from './errors/error-404.component'

import { routes } from './routes'

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpModule,
        RouterModule.forRoot(routes),
        NguiDatetimePickerModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyDHKcbmM0jpW7BOet42_S92KJSr5PYKc5w',
            libraries: ['places']
        })
    ],
    declarations: [
        AppComponent,
        CreateEventComponent,
        SearchEventsComponent,
        DisplayListComponent,
        DisplayMapComponent,
        EventDetailsComponent,
        NavbarComponent,
        Error404Component
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
