import { Car } from "@cars/interfaces/car";

export const carDetailsFeatureKey = 'carDetails';

export interface CarDetailsState {
  car: Car | null
}

export interface State {
  [carDetailsFeatureKey]: CarDetailsState;
}
