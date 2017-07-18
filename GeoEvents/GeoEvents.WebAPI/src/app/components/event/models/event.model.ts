export interface IEvent {
    Id: string
    Name: string
    Description: string
    StartTime: Date,
    EndTime: Date,
    Lat: number,
    Long: number,
    Categories: number[],
    Price: number,
    Capacity: number
}