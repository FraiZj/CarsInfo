import { Car } from "@cars/interfaces/car";

export const carsListFeatureKey = 'carsList';

export interface CarsListState {
  cars: Car[],
  carsCanLoadNext: boolean,
  favoriteCars: Car[],
  favoriteCarsCanLoadNext: boolean,
}

export interface State {
  [carsListFeatureKey]: CarsListState;
}
