export interface IFilter {
    ULat: number,
    ULong: number,
    Radius: number,
    StartTime?: Date,
    EndTime?: Date,
    Category: number,
	
	SearchString: string,
	
	PageNumber: number,
	PageSize: number,
	OrderByString: string,
	OrderIsAscending: boolean
}