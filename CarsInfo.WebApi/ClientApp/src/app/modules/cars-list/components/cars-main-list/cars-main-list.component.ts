import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { Car } from 'app/modules/cars/interfaces/car';
import { OrderBy } from 'app/modules/cars/enums/order-by';
import { FilterWithPaginator } from '../../interfaces/filterWithPaginator';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Filters } from '@cars-filter/enums/filters';

@Component({
  selector: 'cars-main-list',
  template: `<cars-list [filterName]="filterName" [getCars]="getCars"></cars-list>`
})
export class CarsMainListComponent implements OnInit {
  public readonly filterName: Filters = Filters.CarsFilter;
  public getCars!: (filter?: FilterWithPaginator, orderBy?: OrderBy) => Observable<Car[]>

  constructor(private readonly carsService: CarsService) { }

  ngOnInit(): void {
    this.getCars = this.carsService.getCars.bind(this.carsService);
  }
}
