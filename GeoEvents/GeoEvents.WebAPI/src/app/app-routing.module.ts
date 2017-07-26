import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { Error404Component } from "./error/error-404.component";
import { HomeComponent } from "./home/home.component";

@NgModule({
	imports: [
		RouterModule.forRoot([
			{ path: "home", component: HomeComponent },
			{ path: "", redirectTo: "home", pathMatch: "full" },
			{ path: "404", component: Error404Component },
			{ path: "**", redirectTo: "404" }
		]),
	]

})
export class AppRoutingModule {}