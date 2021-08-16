import { Filter } from './../../interfaces/filter';
import { Component, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss']
})
export class CarsFilterComponent {
  private brands: string[] = [];
  private model: string = '';
  @Output() filterEvent = new EventEmitter<Filter>();

  onBrandFilter(brands: string[]): void {
    this.brands = brands;
    this.emitFilterEvent();
  }

  onModelFilter(model: string): void {
    this.model = model;
    this.emitFilterEvent();
  }

  private emitFilterEvent() {
    this.filterEvent.emit({
      brands: this.brands,
      model: this.model,
    });
  }
}
