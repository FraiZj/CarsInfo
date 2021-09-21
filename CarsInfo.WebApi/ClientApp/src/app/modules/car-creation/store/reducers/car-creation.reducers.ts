import { createReducer, on } from '@ngrx/store';
import {CarCreationState} from "@car-creation/store/states/car-creation.state";
import {addCarCreationValidationErrors} from "@car-creation/store/actions/car-creation.actions";

export const initialState: CarCreationState = {
  validationErrors: []
};

export const carCreationReducer = createReducer(
  initialState,
  on(addCarCreationValidationErrors, (state, { validationErrors }) => ({ ...state, validationErrors }))
);
