import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { Filter } from './../../../cars-filter/interfaces/filter';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Subject, Subscription } from 'rxjs';

@Component({
  selector: 'cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit, OnDestroy {
  private static readonly ItemsTakePerLoad: number = 6;
  private static readonly ItemsSkipPerLoad: number = 0;
  private readonly subscriptions: Subscription[] = [];
  private ngUnsubscribe$: Subject<any> = new Subject;
  private filter: FilterWithPaginator = {
    skip: CarsListComponent.ItemsSkipPerLoad,
    take: CarsListComponent.ItemsTakePerLoad
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
    this.filter.skip = CarsListComponent.ItemsSkipPerLoad;
    this.filter.take = CarsListComponent.ItemsTakePerLoad;
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
    this.filter.take = CarsListComponent.ItemsTakePerLoad;
    this.subscriptions.push(this.carsService.getCars(this.filter)
      .subscribe(cars => {
        this.spinner.hide();

        if (cars.length < CarsListComponent.ItemsTakePerLoad) {
          this.notEmptyPost = false;
        }

        this.cars = this.cars.concat(cars);
        this.notscrolly = true;
      }));
  }
}
