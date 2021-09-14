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

export const selectSelectedBrands = createSelector(
  selectCarsFilter,
  (filter) => filter?.brands
);

export const selectFavoriteCarsFilter = createSelector(
  selectFilterState,
  (state) => state.favoriteCarsFilter
);

export const selectBrandsFilterState = createSelector(
  selectCarsFilterState,
  (state) => state.brandFilter
);

export const selectAllBrands = createSelector(
  selectBrandsFilterState,
  (state) => state.brands
);

export const selectFilteredBrands = createSelector(
  selectAllBrands,
  selectSelectedBrands,
  (brands, selectedBrands) => brands.filter(brand => !selectedBrands?.includes(brand.name))
);
