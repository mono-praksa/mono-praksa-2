import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'repeat' })
export class RepeatOnPipe implements PipeTransform {
    transform(value: string, args: string[]): any {
        if (!value) return value;
        var returnString: string;
        var i = parseInt(args[0]);
        if (value === "daily") {
                returnString = "day";
        }
        else {
            returnString = value.substr(0, value.length - 2);
        }
        if (i > 1) {
            return returnString + "s";
        }
        else {
            return returnString;
        }
    }
}