import { CarEditorState } from './../states/car-editor.states';
import { createReducer, on } from '@ngrx/store';
import { fetchCarEditorByIdSuccess } from '../actions/car-editor.actions';

export const initialState: CarEditorState = {
  carEditor: null
};

export const reducer = createReducer(
  initialState,
  on(fetchCarEditorByIdSuccess, (state, { carEditor }) => ({ ...state, carEditor })),
);
