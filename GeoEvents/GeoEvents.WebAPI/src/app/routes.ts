import { Routes } from '@angular/router'

import { CreateEventComponent } from './create-event.component'
import { DisplayListComponent } from './display-list.component'

export const routes: Routes = [
    { path: 'create', component: CreateEventComponent },
    { path: 'display/list', component: DisplayListComponent },
    { path: '', redirectTo: 'create', pathMatch: 'full' }
]