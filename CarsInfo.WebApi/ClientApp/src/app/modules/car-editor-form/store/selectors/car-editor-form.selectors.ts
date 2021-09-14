import { CarEditorFormState, carEditorFormFeatureKey } from './../states/car-editor-form.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCarEditorFormState = createFeatureSelector<CarEditorFormState>(carEditorFormFeatureKey);

export const selectBrands = createSelector(
  selectCarEditorFormState,
  (state) => state.brands
);
