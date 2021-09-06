import { selectCanLoadNextFavoriteCars } from './../../store/selectors/cars-list.selectors';
import { fetchFavoriteCars, loadNextFavoriteCars } from './../../store/actions/cars-list.actions';
import { Component } from '@angular/core';
import { Filters } from '@cars-filter/enums/filters';
import { selectFavoriteCars } from '@cars-list/store/selectors/cars-list.selectors';
import { selectFavoriteCarsFilter } from '@cars-filter/store/selectors/cars-filter.selectors';

@Component({
  selector: 'favorite-cars-list',
  template: `<cars-list [filterName]="filterName" [fetchCars]="fetchCars" [fetchNextCars]="fetchNextCars"
  [selectCanLoad]="selectCanLoad" [selectFilter]="selectFilter" [selectCars]="selectCars"></cars-list>`
})
export class FavoriteCarsListComponent {
  public readonly filterName: Filters = Filters.FavoriteCarsFilter;
  public readonly fetchCars = fetchFavoriteCars;
  public readonly fetchNextCars = loadNextFavoriteCars;
  public readonly selectCanLoad = selectCanLoadNextFavoriteCars;
  public readonly selectFilter = selectFavoriteCarsFilter;
  public readonly selectCars = selectFavoriteCars;
}
