import { carsSortingFeatureKey } from './../states/cars-sorting.state';
import { CarsFilterState } from './../states/index';
import * as fromFilter from './../reducers/cars-filter.reducers';
import * as fromBrandFilter from './../reducers/cars-brand-filter.reducers';
import * as fromCarsSorting from './../reducers/cars-sorting.reducers';
import { filterFeatureKey } from './../states/cars-filter.state';
import { brandFilterFeatureKey } from './../states/cars-brand-filter.state';
import { Action, combineReducers } from "@ngrx/store";

export function reducers(state: CarsFilterState | undefined, action: Action) {
  return combineReducers({
    [filterFeatureKey]: fromFilter.reducer,
    [brandFilterFeatureKey]: fromBrandFilter.reducer,
    [carsSortingFeatureKey]: fromCarsSorting.reducer
  })(state, action);
}
