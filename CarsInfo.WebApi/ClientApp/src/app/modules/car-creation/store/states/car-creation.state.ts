import {ValidationError} from "@core/interfaces/error";

export const carCreationFeatureKey = 'carCreation';

export interface CarCreationState {
  validationErrors: ValidationError[];
}

export interface State {
  [carCreationFeatureKey]: CarCreationState;
}
