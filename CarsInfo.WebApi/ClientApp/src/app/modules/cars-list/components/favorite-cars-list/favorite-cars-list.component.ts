import {selectCanLoadNextFavoriteCars} from './../../store/selectors/cars-list.selectors';
import {fetchFavoriteCars, loadNextFavoriteCars} from './../../store/actions/cars-list.actions';
import {ChangeDetectionStrategy, Component} from '@angular/core';
import {Filters} from '@cars-filter/enums/filters';
import {selectFavoriteCars} from '@cars-list/store/selectors/cars-list.selectors';
import {selectFavoriteCarsFilterWithOrderBy} from '@cars-filter/store/selectors/cars-filter.selectors';

@Component({
  selector: 'favorite-cars-list',
  template: `
    <cars-list [filterName]="filterName" [fetchCars]="fetchCars" [fetchNextCars]="fetchNextCars"
               [selectCanLoad]="selectCanLoad" [selectFilterAndOrderBy]="selectFilterAndOrderBy"
               [selectCars]="selectCars"></cars-list>`,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FavoriteCarsListComponent {
  public readonly filterName: Filters = Filters.FavoriteCarsFilter;
  public readonly fetchCars = fetchFavoriteCars;
  public readonly fetchNextCars = loadNextFavoriteCars;
  public readonly selectCanLoad = selectCanLoadNextFavoriteCars;
  public readonly selectFilterAndOrderBy = selectFavoriteCarsFilterWithOrderBy;
  public readonly selectCars = selectFavoriteCars;
}
