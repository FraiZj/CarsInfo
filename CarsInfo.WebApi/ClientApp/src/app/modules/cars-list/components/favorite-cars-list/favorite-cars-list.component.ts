import { Component, OnInit } from '@angular/core';
import { Filters } from '@cars-filter/enums/filters';
import { OrderBy } from 'app/modules/cars/enums/order-by';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Observable } from 'rxjs';
import { FilterWithPaginator } from '../../interfaces/filterWithPaginator';

@Component({
  selector: 'favorite-cars-list',
  template: `<cars-list [filterName]="filterName" [getCars]="getCars"></cars-list>`
})
export class FavoriteCarsListComponent implements OnInit {
  public readonly filterName: Filters = Filters.FavoriteCarsFilter;
  public getCars!: (filter?: FilterWithPaginator, orderBy?: OrderBy) => Observable<Car[]>

  constructor(private readonly carsService: CarsService) { }

  ngOnInit(): void {
    this.getCars = this.carsService.getUserFavoriteCars.bind(this.carsService);
  }
}
