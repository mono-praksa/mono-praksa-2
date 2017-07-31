import { Injectable } from "@angular/core";

enum CategoryEnum {
    Music = 1,
    Culture = 2,
    Sport = 4,
    Gastro = 8,
    Religious = 16,
    Business = 32,
    Miscellaneous = 64
}

enum DayEnum {
    Sun = 1,
    Mon = 2,
    Tue = 4,
    Wed = 8,
    Thu = 16,
    Fri = 32,
    Sat = 64
}

@Injectable()
export class CategoryService {
    categories: CategoryElement[] = [];
    days: CategoryElement[] = [];
    CategoryEnum = CategoryEnum;
    DayEnum = DayEnum;

    buildCategories(enumName: string, checkLast: boolean = false): void {
        for (let i of this.keys(enumName)) {
            if (enumName == "category") {
                this.categories.push({
                    id: parseInt(i),
                    checked: false
                });
            } else if (enumName == "day") {
                this.days.push({
                    id: parseInt(i),
                    checked: false
                });
            }
        }
        if (checkLast) {
            if (enumName == "category") {
                this.categories[this.categories.length - 1].checked = true;
            } else if(enumName == "day") {
                this.days[this.days.length - 1].checked = true;
            }
        }        
    }

    keys(enumName: string): string[] {
        let keys;

        if (enumName == "category") {
            keys = Object.keys(CategoryEnum);
        } else if(enumName == "day") {
            keys = Object.keys(DayEnum);
        }

        keys.splice(keys.length / 2);
        return keys;
    }
}

class CategoryElement {
    checked: boolean;
    id: number;
}