import { Component, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss']
})
export class CarsFilterComponent {
  @Output() filterEvent = new EventEmitter<string[]>();

  onBrandFilter(brands: string[]) {
    this.filterEvent.emit(brands);
  }
}
