import { CarEditorFormState } from './../states/car-editor-form.state';
import { fetchBrandsSuccess } from './../actions/car-editor-form.actions';
import { createReducer, on } from "@ngrx/store";

export const initialState: CarEditorFormState = {
  brands: []
};

export const reducer = createReducer(
  initialState,
  on(fetchBrandsSuccess, (state, { brands }) => ({ ...state, brands }))
);
