import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";

@Component({
	templateUrl: "./home.component.html"
})
export class HomeComponent implements OnInit {
    searchForm: FormGroup;
    searchTerm: FormControl;

	constructor(private router: Router) { }

    ngOnInit() {
        this.searchTerm = new FormControl("");
        this.searchForm = new FormGroup({
            searchTerm: this.searchTerm
        });
    }

    onSearch(formValues: any) {
        //this.preserveSearchQueryService.searchQuery = formValues.searchTerm;
        this.router.navigate(["event/search"], { queryParams: { searchString: formValues.searchTerm } });
	}
}