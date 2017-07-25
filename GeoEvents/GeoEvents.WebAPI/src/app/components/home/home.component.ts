import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PreserveSearchQuerryService } from '../../shared/preserve-search-querry.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
	templateUrl: './home.component.html',
//	providers: [ PreserveSearchQuerryService ]
})
export class HomeComponent implements OnInit {
	constructor(public _preserveSearchQuerryService: PreserveSearchQuerryService, private _router: Router) { }
    searchForm: FormGroup;
    searchTerm: FormControl;

    ngOnInit() {
        this.searchTerm = new FormControl('');
        this.searchForm = new FormGroup({
            searchTerm: this.searchTerm
        });
    }

    onSearch(formValues: any) {
        this._preserveSearchQuerryService.searchQuerry = formValues.searchTerm;
		this._router.navigate(["/event/search/"]);
	}
}