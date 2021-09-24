import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  name: "duration"
})
export class DurationPipe implements PipeTransform {
  private static readonly TIME_STAMPS: any = {
    year: 31536000,
    month: 2419200,
    week: 604800,
    day: 86400,
    hour: 3600,
    minute: 60
  };

  public transform(value: Date | string): string {
    const commentDate: Date = new Date(value);
    const timeDiff: number = Date.now() - commentDate.getTime();

    if (timeDiff < DurationPipe.TIME_STAMPS.minute) {
      return 'a moment ago';
    }

    for (const key in DurationPipe.TIME_STAMPS) {
      const time: number = DurationPipe.TIME_STAMPS[key] * 1000;
      if (timeDiff >= time) {
        const num: number = Math.floor(timeDiff / time);
        return num + (num > 1 ? ` ${key}s ago` : ` ${key} ago`);
      }
    }

    return 'some time ago';
  }
}
