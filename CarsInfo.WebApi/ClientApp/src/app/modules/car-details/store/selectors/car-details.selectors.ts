import { carDetailsFeatureKey, CarDetailsState } from './../states/car-details.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCarDetailsState = createFeatureSelector<CarDetailsState>(carDetailsFeatureKey);

export const selectCar = createSelector(
  selectCarDetailsState,
  (state) => state.car
);
