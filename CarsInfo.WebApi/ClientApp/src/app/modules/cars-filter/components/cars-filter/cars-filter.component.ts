import { FilterWithPaginator } from '@cars-list/interfaces/filterWithPaginator';
import { ChangeDetectionStrategy, Component, Input, Output, ChangeDetectorRef } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'cars-filter',
  templateUrl: './cars-filter.component.html',
  styleUrls: ['./cars-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsFilterComponent {
  @Input() public filter!: FilterWithPaginator;
  @Output() public filterEvent = new EventEmitter<FilterWithPaginator>();

 constructor (private readonly cdr: ChangeDetectorRef) {}

  public onBrandFilter(brands: string[]): void {
    this.filter = { ...this.filter, brands};
    this.filterEvent.emit(this.filter);
  }

  public onModelFilter(model: string): void {
    this.filter = { ...this.filter, model};
    this.filterEvent.emit(this.filter);
  }
  public clearFilter(): void {
    this.filter = FilterWithPaginator.CreateDefault();
    this.filterEvent.emit(this.filter);
    this.cdr.detectChanges();
  }
}
