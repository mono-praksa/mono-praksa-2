import { Routes } from '@angular/router'

import { CreateComponent } from './create/create.component'
import { SearchEventsComponent } from './search/search-events.component'
import { Error404Component } from './errors/error-404.component'

export const routes: Routes = [
    { path: 'create', component: CreateComponent },
    { path: 'search', component: SearchEventsComponent },
    { path: '', redirectTo: 'create', pathMatch: 'full' },
    { path: '404', component: Error404Component },
    { path: '**', redirectTo: '404' }
]