import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { Filter } from './../../../cars-filter/interfaces/filter';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Subscription } from 'rxjs';
import { ItemsSkipPerLoad, ItemsTakePerLoad } from '../../consts/filter-consts';

@Component({
  selector: 'cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  private filter: FilterWithPaginator = {
    skip: ItemsSkipPerLoad,
    take: ItemsTakePerLoad
  };
  public notEmptyPost = true;
  public notscrolly = true;
  public cars!: Car[];

  constructor(
    private readonly carsService: CarsService,
    private readonly spinner: NgxSpinnerService
  ) { }

  public ngOnInit(): void {
    this.subscriptions.push(this.carsService.getCars()
      .subscribe(cars => this.cars = cars));
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public trackCarsById(index: number, car: Car): number {
    return car.id;
  }

  public getFilteredCars(filter: Filter): void {
    this.filter.brands = filter.brands;
    this.filter.model = filter.model;
    this.filter.skip = ItemsSkipPerLoad;
    this.filter.take = ItemsTakePerLoad;
    this.subscriptions.push(this.carsService.getCars(this.filter)
      .subscribe(cars => this.cars = cars));
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
    this.filter.take = ItemsTakePerLoad;
    this.subscriptions.push(this.carsService.getCars(this.filter)
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
