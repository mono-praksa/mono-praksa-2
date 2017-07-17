import { Component } from '@angular/core'

@Component({
    selector: 'nav-bar',
    templateUrl: 'app/shared/navbar/navbar.component.html',
    styles: [`
        li > a.active {
            background-color: rgb(240,240,240);
            font-weight: bold;
        }
        @media (min-width: 768px) {
          .navbar-nav.navbar-center {
            position: absolute;
            left: 50%;
            transform: translatex(-50%);
          }
        }
    `]
})
export class NavbarComponent { }