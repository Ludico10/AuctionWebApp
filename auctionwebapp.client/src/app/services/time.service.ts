import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { Data } from "@angular/router";

@Injectable()
export class TimeService {

  parseDate(value: any): number | null {
    if ((typeof value === 'string') && (value.indexOf('-') > -1)) {
      const str = value.split('-');
      const dayArray = str[2].split('T');

      const year = Number(str[0]);
      const month = Number(str[1]) - 1;
      const day = Number(dayArray[0]);

      let date = new Date(year, month, day);
      return date.getTime();
    }

    const timestamp = typeof value === 'number' ? value : Date.parse(value);
    return isNaN(timestamp) ? null : timestamp;
  }

  parseTime(value: string): number | null {
    if (value.indexOf(':') > -1) {
      const str = value.split(':');
      const houer = Number(str[0]);
      const minutes = Number(str[1]);
      return (houer * 60 + minutes) * 60000;
    }

    return null;
  }

  getTimeFromMilli(utc: number): Array<number> {
    let result = new Array<number>();
    let timePart = utc % (1000 * 60 * 60 * 24);
    let totalHours = Math.floor(timePart / 1000 / 60 / 60);
    result.push(totalHours);
    let totalMins = Math.floor(timePart / 1000 / 60);
    let minsBefore = totalMins - totalHours * 60;
    result.push(minsBefore);
    let totalSecs = Math.floor(timePart / 1000);
    let secsBefore = totalSecs - totalMins * 60;
    result.push(secsBefore);
    return result;
  }

  getDateFromMilli(utc: number): Date {
    let result = new Date();
    result.setTime(utc);
    return result;
  }

  timeToString(hours: number, mins: number): string {
    let result = "";
    if (hours < 10) result += "0";
    result += hours.toString();
    result += ":";
    if (mins < 10) result += "0";
    result += mins.toString();
    return result;
  }

  dateToString(date: Date): string {
    let result = date.toString();
    result = result.replace('M', ' ');
    result = result.replace('T', ' ');
    result = result.replace('W', ' ');
    result = result.replace('F', ' ');
    result = result.replace('S', ' ');
    let items = result.split('.');
    return items[0];
  }
}
