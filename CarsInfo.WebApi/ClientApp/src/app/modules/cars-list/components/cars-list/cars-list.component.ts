import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { Filter } from './../../../cars-filter/interfaces/filter';
import { Observable } from 'rxjs';
import { CarsService } from '../../../shared/services/cars.service';
import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Car } from 'app/modules/shared/interfaces/car';

@Component({
  selector: 'app-cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit {
  private readonly itemsPerLoad: number = 6;
  cars$!: Observable<Car[]>;
  notEmptyPost = true;
  notscrolly = true;
  cars!: Car[];
  filter: FilterWithPaginator = {
    skip: 0,
    take: this.itemsPerLoad
  };

  constructor(
    private carsService: CarsService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.carsService.getCars()
      .subscribe(cars => this.cars = cars);
  }

  getFilteredCars(filter: Filter): void {
    this.filter.brands = filter.brands;
    this.filter.model = filter.model;
    this.filter.skip = 0;
    this.filter.take = this.itemsPerLoad;
    this.carsService.getCars(this.filter)
      .subscribe(cars => this.cars = cars);
  }

  onScroll() : void {
    if (this.notscrolly && this.notEmptyPost) {
      this.spinner.show();
      this.notscrolly = false;
      this.loadNextCars();
     }
  }

  loadNextCars() {
    this.filter.skip = this.cars.length;
    this.filter.take = this.itemsPerLoad;
    this.carsService.getCars(this.filter)
      .subscribe(cars => {
        this.spinner.hide();

        if (cars.length < this.itemsPerLoad) {
          this.notEmptyPost = false;
        }

        this.cars = this.cars.concat(cars);
        this.notscrolly = true;
      });
  }
}
