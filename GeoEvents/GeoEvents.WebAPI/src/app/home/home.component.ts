import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PreserveSearchQueryService } from '../../shared/preserve-search-query.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
	templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
	constructor(public _preserveSearchQueryService: PreserveSearchQueryService, private _router: Router) { }
    searchForm: FormGroup;
    searchTerm: FormControl;

    ngOnInit() {
        this.searchTerm = new FormControl('');
        this.searchForm = new FormGroup({
            searchTerm: this.searchTerm
        });
    }

    onSearch(formValues: any) {
        this._preserveSearchQueryService.searchQuery = formValues.searchTerm;
		this._router.navigate(["/event/search/"]);
	}
}