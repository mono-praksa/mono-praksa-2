import { Injectable } from '@angular/core';

enum CategoryEnum {
    Music = 1,
    Culture = 2,
    Sport = 4,
    Gastro = 8,
    Religious = 16,
    Business = 32,
    Miscellaneous = 64
}

@Injectable()
export class CategoryService {
    CategoryEnum = CategoryEnum;
    categories: ICategoryElement[] = [];

    buildCategories(): void {
        for (let i of this.keys()) {
            this.categories.push({
                id: parseInt(i),
                checked: false
            });
        }
        this.categories[this.categories.length - 1].checked = true;
    }

    keys(): string[] {
        var keys = Object.keys(CategoryEnum);
        keys.splice(keys.length / 2);
        return keys;
    }
}

interface ICategoryElement {
    id: number,
    checked: boolean
}