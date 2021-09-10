import { selectCanLoadNextCars, selectCars } from './../../store/selectors/cars-list.selectors';
import { fetchCars, loadNextCars } from './../../store/actions/cars-list.actions';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Filters } from '@cars-filter/enums/filters';
import { selectCarsFilter } from '@cars-filter/store/selectors/cars-filter.selectors';

@Component({
  selector: 'cars-main-list',
  template: `<cars-list [filterName]="filterName" [fetchCars]="fetchCars" [fetchNextCars]="fetchNextCars"
    [selectCanLoad]="selectCanLoad" [selectFilter]="selectFilter" [selectCars]="selectCars"></cars-list>`,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsMainListComponent {
  public readonly filterName: Filters = Filters.CarsFilter;
  public readonly fetchCars = fetchCars;
  public readonly fetchNextCars = loadNextCars;
  public readonly selectCanLoad = selectCanLoadNextCars;
  public readonly selectFilter = selectCarsFilter;
  public readonly selectCars = selectCars;
}
