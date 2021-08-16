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
  @Output() public filterEvent = new EventEmitter<Filter>();

  public onBrandFilter(brands: string[]): void {
    this.brands = brands;
    this.emitFilterEvent();
  }

  public onModelFilter(model: string): void {
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
