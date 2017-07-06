import { NgModule }      from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'

import { AppComponent } from './app.component'
import { CreateEventComponent } from './create-event.component'
import { NguiDatetimePickerModule } from '@ngui/datetime-picker'

@NgModule({
    imports: [
        BrowserModule,
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
