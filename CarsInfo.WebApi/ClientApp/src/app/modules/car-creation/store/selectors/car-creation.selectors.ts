import {createFeatureSelector, createSelector} from "@ngrx/store";
import {carCreationFeatureKey, CarCreationState} from "@car-creation/store/states/car-creation.state";

export const selectCarCreationState = createFeatureSelector<CarCreationState>(carCreationFeatureKey);

export const selectCarCreationValidationErrors = createSelector(
  selectCarCreationState,
  (state) => state.validationErrors
);
