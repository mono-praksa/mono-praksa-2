import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { HomeComponent } from "./components/home/home.component";
import { Error404Component } from "./components/error/error-404.component";

@NgModule({
	imports: [
		RouterModule.forRoot([
		{ path: 'home', component: HomeComponent },
		{ path: '', redirectTo: 'home', pathMatch: 'full' },
		{ path: '404', component: Error404Component },
		{ path: '**', redirectTo: '404' }
		]),
	]

})
export class AppRoutingModule {}