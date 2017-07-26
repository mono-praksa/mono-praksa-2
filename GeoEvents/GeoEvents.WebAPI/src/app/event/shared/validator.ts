import { FormControl, FormGroup } from "@angular/forms"

function stringToDateArray(_date: string, _format: string, _delimiter: string) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var formatedDate = [];

    if (monthIndex >= 0) {
        formatedDate[monthIndex] = +dateItems[monthIndex];
    }
    if (dayIndex >= 0) {
        formatedDate[dayIndex] = +dateItems[dayIndex];
    }
    if (yearIndex >= 0) {
        formatedDate[yearIndex] = +dateItems[yearIndex];
    }

    return formatedDate.filter(function (n) { return n != undefined });
}

function stringToTimeArray(_time: string, _format: string, _delimiter: string) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var timeItems = _time.split(_delimiter);
    var hourIndex = formatItems.indexOf("hh");
    var minuteIndex = formatItems.indexOf("mm");
    var secondIndex = formatItems.indexOf("ss");
    var formatedTime = [];

    if (hourIndex >= 0) {
        formatedTime[hourIndex] = +timeItems[hourIndex];
    }
    if (minuteIndex >= 0) {
        formatedTime[minuteIndex] = +timeItems[minuteIndex];
    }
    if (secondIndex >= 0) {
        formatedTime[secondIndex] = +timeItems[secondIndex];
    }

    return formatedTime.filter(function (n) { return n != undefined });
}

export function endDateBeforeStartDate(startKey: string, endKey: string) {
    return (formGroup: FormGroup): { [key: string]: any } => {
        let start = formGroup.controls[startKey];
        let end = formGroup.controls[endKey];

        if ((start.value != "" && start.value != null) && (end.value != "" && end.value != null)) {
            var startTimeDate = start.value.split(" ");
            var startDate = startTimeDate[0];
            var startTime = startTimeDate[1];

            var endTimeDate = end.value.split(" ");
            var endDate = endTimeDate[0];
            var endTime = endTimeDate[1];

            var startDateComponents = stringToDateArray(startDate, "yyyy-mm-dd", "-");
            var startTimeComponents = stringToTimeArray(startTime, "hh:mm", ":");
            var endDateComponents = stringToDateArray(endDate, "yyyy-mm-dd", "-");
            var endTimeComponents = stringToTimeArray(endTime, "hh:mm", ":");

            var startDateTime = new Date(startDateComponents[0], startDateComponents[1] - 1, startDateComponents[2], startTimeComponents[0], startTimeComponents[1]);
            var endDateTime = new Date(endDateComponents[0], endDateComponents[1] - 1, endDateComponents[2], endTimeComponents[0], endTimeComponents[1]);

            if (startDateTime >= endDateTime) {
                return {
                    endDateBeforeStartDate: true
                }
            }
        }
        return null;
    }
}

export function needBothOrNeitherOfAddressAndRadius(addressKey: string, radiusKey: string) {
    return (group: FormGroup): { [key: string]: any } => {
        let address = group.controls[addressKey];
        let radius = group.controls[radiusKey];

        if ((address.value != null) != (radius.value != null && radius.value > 0)) {
            return {
                needBothOrNeitherOfAddressAndRadius: true
            }
        }
        else {
            return null;
        }
    }
}

export function uniqueName(names: string[]){
    return (control: FormControl): { [key: string]: any } => {
        if (!names) return null

        if (names.some(function (name) {
            return name == control.value
        })) {
            return { "uniqueName": "invalid" }
        }
        return null
    }
}