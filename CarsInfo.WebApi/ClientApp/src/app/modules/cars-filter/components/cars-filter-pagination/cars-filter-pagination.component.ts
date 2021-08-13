import { Pagination } from './../../interfaces/pagination';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'cars-filter-pagination',
  templateUrl: './cars-filter-pagination.component.html',
  styleUrls: ['./cars-filter-pagination.component.scss']
})
export class CarsFilterPaginationComponent {
  @Input() pagination: Pagination = {
    skip: 0,
    take: 6
  };
  @Output() paginationChange = new EventEmitter<Pagination>();
  page: number = 1;
  carsPerPage: number[] = [6, 12, 18];

  onCarsPerPageChange(value: number) {
    this.pagination.take = value;
    this.paginationChange.emit(this.pagination)
  }
}
