import { CarEditor } from '@cars/interfaces/car-editor';
import { createAction, props } from "@ngrx/store";
import {ValidationError} from "@core/interfaces/error";

export const createCar = createAction(
  '[Car Creation] Create Car',
  props<{ car: CarEditor }>()
);

export const createCarSuccess = createAction(
  '[Car Creation] Create Car Success'
);

export const addCarCreationValidationErrors = createAction(
  '[Car Creation] Add Validation Errors',
  props<{ validationErrors: ValidationError[] }>()
);
