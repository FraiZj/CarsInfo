import { CarEditor } from './../../../cars/interfaces/car-editor';
import { createAction, props } from "@ngrx/store";

export const createCar = createAction(
  '[Car Creation] Create Car',
  props<{ car: CarEditor }>()
);

export const createCarSuccess = createAction(
  '[Car Creation] Create Car Success'
);

export const createCarFailed = createAction(
  '[Car Creation] Create Car Failed'
);