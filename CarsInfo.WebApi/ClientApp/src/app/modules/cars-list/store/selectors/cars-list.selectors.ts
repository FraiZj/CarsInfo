import { CarsListState, carsListFeatureKey } from './../states/cars-list.states';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCarsListState = createFeatureSelector<CarsListState>(carsListFeatureKey);

export const selectCars = createSelector(
  selectCarsListState,
  (state) => state.cars
);

export const selectFavoriteCars = createSelector(
  selectCarsListState,
  (state) => state.favoriteCars
);

export const selectCanLoadNextCars = createSelector(
  selectCarsListState,
  (state) => state.carsCanLoadNext
);

export const selectCanLoadNextFavoriteCars = createSelector(
  selectCarsListState,
  (state) => state.favoriteCarsCanLoadNext
);

export const favoriteCarsIds = createSelector(
  selectCarsListState,
  (state) => state.favoriteCarsIds
);
