import { NgModule }      from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { ReactiveFormsModule } from '@angular/forms'

import { AppComponent } from './app.component'
import { CreateEventComponent } from './create-event.component'
import { NguiDatetimePickerModule } from '@ngui/datetime-picker'

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        NguiDatetimePickerModule
    ],
    declarations: [
        AppComponent,
        CreateEventComponent
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
