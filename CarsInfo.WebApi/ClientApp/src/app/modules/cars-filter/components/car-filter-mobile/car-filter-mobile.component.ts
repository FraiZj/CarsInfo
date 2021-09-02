import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FilterWithPaginator } from 'app/modules/cars-list/interfaces/filterWithPaginator';
import { OrderBy } from 'app/modules/cars/enums/order-by';

@Component({
  selector: 'car-filter-mobile',
  templateUrl: './car-filter-mobile.component.html',
  styleUrls: ['./car-filter-mobile.component.scss']
})
export class CarFilterMobileComponent {
  @Input() public filter!: FilterWithPaginator;
  @Input() public orderBy: OrderBy = OrderBy.BrandNameAsc;
  @Output() public orderByChange: EventEmitter<OrderBy> = new EventEmitter<OrderBy>();
  @Output() public filterEvent = new EventEmitter<FilterWithPaginator>();
  @Output() public closeFilterEvent = new EventEmitter();

  public onFilter(filter: FilterWithPaginator): void {
    this.filter = filter;
    this.filterEvent.emit(this.filter);
  }

  public onClick(event: MouseEvent) {
    if ((event.target as HTMLElement)?.classList.contains('car-filter-mobile-bg')) {
      this.onClose();
    }
  }

  public onOrderByChange(orderBy: OrderBy): void {
    this.orderBy = orderBy;
    this.orderByChange.emit(this.orderBy);
  }

  public onClose() {
    this.closeFilterEvent.emit();
  }
}
