import { CarsFilterState, carsFilterFeatureKey } from './../states/index';
import { createFeatureSelector, createSelector } from '@ngrx/store';

export const selectCarsFilterState = createFeatureSelector<CarsFilterState>(carsFilterFeatureKey);

export const selectFilterState = createSelector(
  selectCarsFilterState,
  (state) => state.filter
);

export const selectCarsFilter = createSelector(
  selectFilterState,
  (state) => state.carsFilter
);

export const selectFavoriteCarsFilter = createSelector(
  selectFilterState,
  (state) => state.favoriteCarsFilter
);

export const selectBrandsFilterState = createSelector(
  selectCarsFilterState,
  (state) => state.brandFilter
);

export const selectBrandsFilter = createSelector(
  selectBrandsFilterState,
  (state) => state.brands
);
