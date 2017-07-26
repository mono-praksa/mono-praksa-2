import { Component } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

@Component({
  selector: "geo-events",
  template: `
    <nav-bar></nav-bar>
    <div class="container">
        <router-outlet></router-outlet>
    </div>
  `
})
export class AppComponent  { }
