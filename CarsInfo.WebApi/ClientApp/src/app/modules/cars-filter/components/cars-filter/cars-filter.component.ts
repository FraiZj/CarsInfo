import { Pagination } from './../../interfaces/pagination';
import { Filter } from './../../interfaces/filter';
import { ChangeDetectionStrategy, Component, OnChanges, Output, SimpleChanges } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss']
})
export class CarsFilterComponent {
  private brands: string[] = [];
  private model: string = '';
  pagination: Pagination= {
    skip: 0,
    take: 6
  };
  @Output() filterEvent = new EventEmitter<Filter>();

  onBrandFilter(brands: string[]): void {
    this.brands = brands;
    this.emitFilterEvent();
  }

  onModelFilter(model: string): void {
    this.model = model;
    this.emitFilterEvent();
  }

  onPageFilter(pagination: Pagination): void {
    this.pagination = pagination;
    this.emitFilterEvent();
  }

  private emitFilterEvent() {
    this.filterEvent.emit({
      brands: this.brands,
      model: this.model,
      skip: this.pagination.skip,
      take: this.pagination.take
    });
  }
}
