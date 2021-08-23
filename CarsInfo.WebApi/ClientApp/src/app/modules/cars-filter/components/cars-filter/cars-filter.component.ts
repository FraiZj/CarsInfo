import { FilterWithPaginator } from 'app/modules/cars-list/interfaces/filterWithPaginator';
import { Component, Input, Output, OnInit } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss']
})
export class CarsFilterComponent {
  @Input() public filter!: FilterWithPaginator;
  @Output() public filterEvent = new EventEmitter<FilterWithPaginator>();

  public onBrandFilter(brands: string[]): void {
    this.filter.brands = brands;
    this.filterEvent.emit(this.filter);
  }

  public onModelFilter(model: string): void {
    this.filter.model = model;
    this.filterEvent.emit(this.filter);
  }

  public clearFilter():void {
    this.filter = FilterWithPaginator.CreateDefault();
    this.filterEvent.emit(this.filter);
  }
}
