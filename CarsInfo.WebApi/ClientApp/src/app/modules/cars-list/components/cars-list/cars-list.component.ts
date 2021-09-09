import { AsyncPipe } from '@angular/common';
import { Filters } from '@cars-filter/enums/filters';
import { map } from 'rxjs/operators';
import { ItemsSkipPerLoad } from './../../consts/filter-consts';
import { OrderBy } from '@cars/enums/order-by';
import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Car } from 'app/modules/cars/interfaces/car';
import { Observable, Subscription } from 'rxjs';
import { ItemsTakePerLoad } from '../../consts/filter-consts';
import { DefaultProjectorFn, MemoizedSelector, Store } from '@ngrx/store';
import * as FilterActions from '@cars-filter/store/actions/cars-filter.actions';
import { Filter } from '@cars-filter/interfaces/filter';

type CanLoadNextCarsSelector = MemoizedSelector<object, boolean, DefaultProjectorFn<boolean>>;
type CarsSelector = MemoizedSelector<object, Car[], DefaultProjectorFn<Car[]>>;
type FilterSelector = MemoizedSelector<object, Filter | null, DefaultProjectorFn<Filter | null>>;

@Component({
  selector: 'cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit, OnDestroy {
  @Input() public fetchCars!: any;
  @Input() public fetchNextCars!: any;
  @Input() public selectCanLoad!: CanLoadNextCarsSelector;
  @Input() public selectFilter!: FilterSelector;
  @Input() public selectCars!: CarsSelector;
  @Input() public filterName!: Filters;
  @Input() public getCars!: (filter?: FilterWithPaginator, orderBy?: OrderBy) => Observable<Car[]>;
  private readonly subscriptions: Subscription[] = [];
  public filter!: FilterWithPaginator;
  public orderBy: OrderBy = OrderBy.BrandNameAsc;
  public notEmptyPost = true;
  public notscrolly = true;
  public cars$!: Observable<Car[]>;
  public mobileFilterOpened: boolean = false;

  constructor(
    private readonly store: Store,
    private readonly spinner: NgxSpinnerService,
    private readonly asyncPipe: AsyncPipe
  ) { }

  public ngOnInit(): void {
    this.cars$ = this.store.select(this.selectCars);

    this.store.select(this.selectFilter).pipe(
      map(filter => {
        this.filter = FilterWithPaginator.CreateDefault();
        this.filter.brands = filter?.brands ?? [];
        this.filter.model = filter?.model ?? '';
      })
    ).subscribe(() => this.store.dispatch(this.fetchCars({
      filter: this.filter,
      orderBy: this.orderBy
    })));
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public trackBy(index: number, car: Car): string {
    return Object.values(car).toString();
  }

  public getFilteredCars(filter: FilterWithPaginator): void {
    this.filter = filter;
    this.filter.skip = ItemsSkipPerLoad;
    this.store.dispatch(FilterActions.saveFilter({
      filterName: this.filterName,
      filter: this.filter
    }));
    this.notEmptyPost = true;
    this.store.dispatch(this.fetchCars({
      filter: this.filter,
      orderBy: this.orderBy
    }));
  }

  public orderByChange(orderBy: OrderBy) {
    this.notEmptyPost = true;
    this.orderBy = orderBy;
    this.filter = {
      ...this.filter,
      skip: ItemsSkipPerLoad,
      take: ItemsTakePerLoad
    };
    this.store.dispatch(this.fetchCars({
      filter: this.filter,
      orderBy: this.orderBy
    }));
  }

  public openMobileFilter() {
    this.mobileFilterOpened = true;
  }

  public closeMobileFilter() {
    this.mobileFilterOpened = false;
  }

  public onScroll(): void {
    if (this.notscrolly && this.notEmptyPost) {
      this.spinner.show();
      this.notscrolly = false;
      this.loadNextCars();
    }
  }

  public loadNextCars(): void {
    this.filter = {
      ...this.filter,
      skip: this.asyncPipe.transform(this.cars$)?.length ?? 0
    };

    this.spinner.hide();
    this.store.dispatch(this.fetchNextCars({
      filter: this.filter,
      orderBy: this.orderBy
    }));
    this.notscrolly = true;
    this.store.select(this.selectCanLoad).subscribe(can => this.notEmptyPost = can as boolean);
  }
}
