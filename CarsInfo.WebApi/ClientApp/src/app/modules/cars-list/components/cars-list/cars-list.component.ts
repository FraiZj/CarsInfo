import { ItemsSkipPerLoad } from './../../consts/filter-consts';
import { OrderBy } from './../../../cars/enums/order-by';
import { FilterService } from './../../../cars/services/filter.service';
import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Car } from 'app/modules/cars/interfaces/car';
import { Observable, Subscription } from 'rxjs';
import { ItemsTakePerLoad } from '../../consts/filter-consts';

@Component({
  selector: 'cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit, OnDestroy {
  @Input() public filterName!: string;
  @Input() public getCars!: (filter?: FilterWithPaginator, orderBy?: OrderBy) => Observable<Car[]>;
  private readonly subscriptions: Subscription[] = [];
  public filter!: FilterWithPaginator;
  public orderBy: OrderBy = OrderBy.BrandNameAsc;
  public notEmptyPost = true;
  public notscrolly = true;
  public cars!: Car[];
  public mobileFilterOpened: boolean = false;

  constructor(
    private readonly filterService: FilterService,
    private readonly spinner: NgxSpinnerService
  ) { }

  public ngOnInit(): void {
    const savedFilter = this.filterService.getFilter(this.filterName);
    this.filter = FilterWithPaginator.CreateDefault();

    if (savedFilter != null) {
      this.filter.brands = savedFilter.brands;
      this.filter.model = savedFilter.model;
    }

    this.orderBy = this.filterService.getOrderBy(this.filterName) ?? OrderBy.BrandNameAsc;

    this.subscriptions.push(this.getCars(this.filter, this.orderBy)
      .subscribe(cars => this.cars = cars));
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public trackCarsById(index: number, car: Car): number {
    return car.id;
  }

  public getFilteredCars(filter: FilterWithPaginator): void {
    this.filter = filter;
    this.filter.skip = ItemsSkipPerLoad;
    this.filterService.saveFilter(this.filterName, {
      brands: this.filter.brands,
      model: this.filter.model
    });
    this.notEmptyPost = true;
    this.subscriptions.push(this.getCars(this.filter, this.orderBy)
      .subscribe(cars => this.cars = cars));
  }

  public orderByChange(orderBy: OrderBy) {
    this.notEmptyPost = true;
    this.orderBy = orderBy;
    this.filter.skip = ItemsSkipPerLoad;
    this.filter.take = ItemsTakePerLoad;
    this.subscriptions.push(this.getCars(this.filter, this.orderBy)
      .subscribe(cars => this.cars = cars));
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
    this.filter.skip = this.cars.length;
    this.subscriptions.push(this.getCars(this.filter, this.orderBy)
      .subscribe(cars => {
        this.spinner.hide();

        if (cars.length < ItemsTakePerLoad) {
          this.notEmptyPost = false;
        }

        this.cars = this.cars.concat(cars);
        this.notscrolly = true;
      }));
  }
}
