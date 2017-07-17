import { Component} from '@angular/core';
import { Router } from '@angular/router';
import { PreserveSearchQuerryService } from '../../shared/preserve-search-querry.service';
import { FormsModule } from '@angular/forms';

@Component({
	templateUrl: './home.component.html',
//	providers: [ PreserveSearchQuerryService ]
})
export class HomeComponent {
	constructor(public _preserveSearchQuerryService: PreserveSearchQuerryService, private _router: Router) { }
	
	searchTerm: string;
	
	onSearch( ) {
		this._preserveSearchQuerryService.searchQuerry = this.searchTerm;
		this._router.navigate(["/event/search/"]);
	}
}