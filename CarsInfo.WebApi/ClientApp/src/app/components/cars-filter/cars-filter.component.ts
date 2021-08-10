import { Component, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss']
})
export class CarsFilterComponent {
  @Output() filterEvent = new EventEmitter();

  constructor() { }

  onFilter(event: Event): void {
    const value = (<HTMLInputElement>event.target).value;
    this.filterEvent.emit(value);
  }
}
