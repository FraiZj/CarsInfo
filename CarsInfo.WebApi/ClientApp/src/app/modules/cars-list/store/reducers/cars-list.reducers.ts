import * as CarsListActions from './../actions/cars-list.actions';
import { createReducer, on } from '@ngrx/store';
import { CarsListState } from './../states/cars-list.states';

export const initialState: CarsListState = {
  cars: [],
  carsCanLoadNext: true,
  favoriteCars: [],
  favoriteCarsCanLoadNext: true,
  favoriteCarsIds: []
};

export const reducer = createReducer(
  initialState,
  on(CarsListActions.fetchCarsSuccess, (state, { cars }) => ({ ...state, cars })),
  on(CarsListActions.loadNextCarsSuccess, (state, { cars }) => ({ ...state, cars: state.cars.concat(cars) })),
  on(CarsListActions.canLoadNextCars, (state, { can }) => ({ ...state, carsCanLoadNext: can })),
  on(CarsListActions.fetchFavoriteCarsSuccess, (state, { cars }) => ({ ...state, favoriteCars: cars })),
  on(CarsListActions.loadNextFavoriteCarsSuccess, (state, { cars }) => ({ ...state, favoriteCars: state.favoriteCars.concat(cars) })),
  on(CarsListActions.canLoadNextFavoriteCars, (state, { can }) => ({ ...state, favoriteCarsCanLoadNext: can })),
  on(CarsListActions.fetchFavoriteCarsIdsSuccess, (state, { ids }) => ({ ...state, favoriteCarsIds: ids })),
);
