import { Component, OnInit, OnDestroy } from '@angular/core';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { FilterService } from 'app/modules/cars/services/filter.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';
import { ItemsTakePerLoad } from '../../consts/filter-consts';
import { FilterWithPaginator } from '../../interfaces/filterWithPaginator';

@Component({
  selector: 'favorite-cars-list',
  templateUrl: '../cars-list/cars-list.component.html',
  styleUrls: ['../cars-list/cars-list.component.scss']
})
export class FavoriteCarsListComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  private readonly filterName: string = 'favorite-cars-filter';
  public filter!: FilterWithPaginator;
  public notEmptyPost = true;
  public notscrolly = true;
  public cars!: Car[];
  public mobileFilterOpened: boolean = false;

  constructor(
    private readonly carsService: CarsService,
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

    this.subscriptions.push(this.carsService.getUserFavoriteCars(this.filter)
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
    this.filterService.saveFilter(this.filterName, {
      brands: this.filter.brands,
      model: this.filter.model
    });
    this.subscriptions.push(this.carsService.getUserFavoriteCars(this.filter)
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
    this.subscriptions.push(this.carsService.getUserFavoriteCars(this.filter)
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
