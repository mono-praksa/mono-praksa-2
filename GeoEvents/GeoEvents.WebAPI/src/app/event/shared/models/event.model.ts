export class Event {
    Capacity: number;
    Categories: number[];
    Custom: string;
    CustomModel: CustomAttribute[];
    Description: string;
    EndTime: Date;
    Id: string;
    Latitude: number;
    LocationId: string;
    Longitude: number;
    Name: string;
    Occurence: string;
    Price: number;
    RateCount: number;
    Rating: number;
    Reserved: number;
    StartTime: Date;

    // attributes for reccuring events
    RepeatCount?: number;
    RepeatEvery?: string;
    RepeatOn?: string;
}

export class CustomAttribute {
    key: string;
    values: string[];
}