import { fetchCarByIdSuccess, fetchCarByIdFailed } from './../actions/car-details.actions';
import { createReducer, on } from '@ngrx/store';
import { CarDetailsState } from './../states/car-details.state';

export const initialState: CarDetailsState = {
  car: null
};

export const reducer = createReducer(
  initialState,
  on(fetchCarByIdSuccess, (state, { car }) => ({ ...state, car })),
  on(fetchCarByIdFailed, (state) => ({ ...state, car: null }))
);
