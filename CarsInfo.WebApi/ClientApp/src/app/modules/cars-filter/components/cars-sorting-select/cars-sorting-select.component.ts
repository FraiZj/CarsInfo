import { OrderBy } from '@cars/enums/order-by';
import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'cars-sorting-select',
  templateUrl: './cars-sorting-select.component.html',
  styleUrls: ['./cars-sorting-select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsSortingSelectComponent {
  @Input() public orderBy: OrderBy = OrderBy.BrandNameAsc;
  @Output() public orderByChange: EventEmitter<OrderBy> = new EventEmitter<OrderBy>();
  public sortingOptions: { text: string, value: OrderBy }[] = [
    { text: "Brand Asc", value: OrderBy.BrandNameAsc },
    { text: "Brand Desc", value: OrderBy.BrandNameDesc },
  ]

  public onClick(orderBy: OrderBy): void {
    this.orderBy = orderBy;
    this.orderByChange.emit(this.orderBy);
  }
}
