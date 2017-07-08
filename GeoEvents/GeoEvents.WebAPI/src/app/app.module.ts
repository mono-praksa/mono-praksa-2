import { NgModule }      from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { ReactiveFormsModule } from '@angular/forms'
import { HttpModule } from '@angular/http'
import { NguiDatetimePickerModule } from '@ngui/datetime-picker'

import { AppComponent } from './app.component'
import { CreateEventComponent } from './create-event.component'
import { DisplayListComponent } from './display-list.component'

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpModule,
        NguiDatetimePickerModule
    ],
    declarations: [
        AppComponent,
        CreateEventComponent,
        DisplayListComponent
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
