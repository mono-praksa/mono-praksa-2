import { NgModule }      from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { ReactiveFormsModule } from '@angular/forms'
import { HttpModule } from '@angular/http'
import { RouterModule } from '@angular/router'
import { NguiDatetimePickerModule } from '@ngui/datetime-picker'

import { AppComponent } from './app.component'
import { CreateEventComponent } from './create-event.component'
import { DisplayListComponent } from './display-list.component'
import { NavbarComponent } from './navbar/navbar.component'

import { routes } from './routes'

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpModule,
        RouterModule.forRoot(routes),
        NguiDatetimePickerModule
    ],
    declarations: [
        AppComponent,
        CreateEventComponent,
        DisplayListComponent,
        NavbarComponent
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
