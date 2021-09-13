import { createReducer } from "@ngrx/store";

export const carCreationFeatureKey = 'carCreation';

export interface CarCreationtate { }

export interface State {
  [carCreationFeatureKey]: CarCreationtate;
}

export const initialState: CarCreationtate = {};

export const reducer = createReducer(
  initialState
);