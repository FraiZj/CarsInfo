import { createFeatureSelector, createSelector } from '@ngrx/store';
import { FilterState } from './../states/cars-filter.state';
import * as fromFilter from './../states/cars-filter.state';

export const selectFilterState = createFeatureSelector<FilterState>(fromFilter.filterFeatureKey);

export const selectCarsFilter = createSelector(
  selectFilterState,
  (state) => state.carsFilter
);

export const selectFavoriteCarsFilter = createSelector(
  selectFilterState,
  (state) => state.favoriteCarsFilter
);
