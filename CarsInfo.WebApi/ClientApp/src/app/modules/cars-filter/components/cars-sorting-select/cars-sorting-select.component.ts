import { OrderBy } from './../../../cars/enums/order-by';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-sorting-select',
  templateUrl: './cars-sorting-select.component.html',
  styleUrls: ['./cars-sorting-select.component.scss']
})
export class CarsSortingSelectComponent {
  @Input() public orderBy: OrderBy = OrderBy.BrandNameAsc;
  @Output() public orderByChange: EventEmitter<OrderBy> = new EventEmitter<OrderBy>();
  public sortingOptions: { text: string, value: OrderBy }[] = [
    { text: "Brand Asc", value: OrderBy.BrandNameAsc },
    { text: "Brand Desc", value: OrderBy.BrandNameDesc },
  ]

  public onSelect(target: EventTarget | null): void {
    this.orderBy = (target as HTMLInputElement).value as OrderBy;
    this.orderByChange.emit(this.orderBy);
  }

  public onClick(orderBy: OrderBy): void {
    this.orderBy = orderBy;
    this.orderByChange.emit(this.orderBy);
  }
}
