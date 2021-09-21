import { CarEditorState, carEditorFeatureKey } from '../states/car-editor.states';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCarEditorState = createFeatureSelector<CarEditorState>(carEditorFeatureKey);

export const selectCarEditor = createSelector(
  selectCarEditorState,
  (state) => state.carEditor
);

export const selectCarEditorValidationErrors = createSelector(
  selectCarEditorState,
  (state) => state.validationErrors
);
