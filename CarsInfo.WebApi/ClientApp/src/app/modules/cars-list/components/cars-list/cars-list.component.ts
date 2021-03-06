import {saveOrderBy} from '@cars-filter/store/actions/cars-sorting.actions';
import {selectLoggedInOnly} from '@auth/store/selectors/auth.selectors';
import * as CarsListActions from './../../store/actions/cars-list.actions';
import {AsyncPipe} from '@angular/common';
import {Filters} from '@cars-filter/enums/filters';
import {ItemsSkipPerLoad} from './../../consts/filter-consts';
import {OrderBy} from '@cars/enums/order-by';
import {FilterWithPaginator} from './../../interfaces/filterWithPaginator';
import {ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';
import {Car} from 'app/modules/cars/interfaces/car';
import {Observable, Subscription} from 'rxjs';
import {ItemsTakePerLoad} from '../../consts/filter-consts';
import {DefaultProjectorFn, MemoizedSelector, Store} from '@ngrx/store';
import * as FilterActions from '@cars-filter/store/actions/cars-filter.actions';
import {Filter} from '@cars-filter/interfaces/filter';

type CanLoadNextCarsSelector = MemoizedSelector<object, boolean, DefaultProjectorFn<boolean>>;
type CarsSelector = MemoizedSelector<object, Car[], DefaultProjectorFn<Car[]>>;
type FilterSelector = MemoizedSelector<object, {
  filter: Filter | null;
  orderBy: OrderBy;
}, DefaultProjectorFn<{
  filter: Filter | null;
  orderBy: OrderBy;
}>>;

@Component({
  selector: 'cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsListComponent implements OnInit, OnDestroy {
  @Input() public fetchCars!: any;
  @Input() public fetchNextCars!: any;
  @Input() public selectCanLoad!: CanLoadNextCarsSelector;
  @Input() public selectFilterAndOrderBy!: FilterSelector;
  @Input() public selectCars!: CarsSelector;
  @Input() public filterName!: Filters;
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
    private readonly asyncPipe: AsyncPipe,
    private readonly cdr: ChangeDetectorRef
  ) {
  }

  public ngOnInit(): void {
    this.cars$ = this.store.select(this.selectCars);
    this.dispatchFetchingFavoriteCarsIds();
    this.dispatchFetchingCarsByFilter();
  }

  private dispatchFetchingFavoriteCarsIds(): void {
    this.subscriptions.push(
      this.store.pipe(selectLoggedInOnly).subscribe(
        () => {
          this.notEmptyPost = true;
          this.notscrolly = true;
          this.store.dispatch(CarsListActions.fetchFavoriteCarsIds())
        }
      )
    );
  }

  private dispatchFetchingCarsByFilter(): void {
    this.subscriptions.push(
      this.store.select(this.selectFilterAndOrderBy).subscribe(
        value => {
          this.filter = FilterWithPaginator.CreateDefault();
          this.filter.brands = value.filter?.brands ?? [];
          this.filter.model = value.filter?.model ?? '';
          this.orderBy = value.orderBy;
          this.store.dispatch(this.fetchCars({
            filter: this.filter,
            orderBy: this.orderBy
          }))
        }
      )
    );
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
    this.store.dispatch(saveOrderBy({
      filterName: this.filterName,
      orderBy: this.orderBy
    }));
    this.store.dispatch(this.fetchCars({
      filter: this.filter,
      orderBy: this.orderBy
    }));
  }

  public openMobileFilter() {
    this.mobileFilterOpened = true;
    this.cdr.detectChanges();
    this.store.dispatch(saveOrderBy({
      filterName: this.filterName,
      orderBy: this.orderBy
    }));
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
    this.subscriptions.push(
      this.store.select(this.selectCanLoad).subscribe(can => this.notEmptyPost = can as boolean)
    );
  }
}
