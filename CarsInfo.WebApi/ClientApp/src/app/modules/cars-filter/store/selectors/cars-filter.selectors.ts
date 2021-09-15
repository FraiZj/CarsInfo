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

export const selectCarsSortingState = createSelector(
  selectCarsFilterState,
  (state) => state.carsSorting
);

export const selectCarsListOrderBy = createSelector(
  selectCarsSortingState,
  (state) => state.carsListOrderBy
);

export const selectFavoriteCarsListOrderBy = createSelector(
  selectCarsSortingState,
  (state) => state.favoriteCarsListOrderBy
);

export const selectCarsFilterWithOrderBy = createSelector(
  selectCarsFilter,
  selectCarsListOrderBy,
  (filter, orderBy) => ({ filter, orderBy })
);

export const selectFavoriteCarsFilterWithOrderBy = createSelector(
  selectFavoriteCarsFilter,
  selectFavoriteCarsListOrderBy,
  (filter, orderBy) => ({ filter, orderBy })
);
