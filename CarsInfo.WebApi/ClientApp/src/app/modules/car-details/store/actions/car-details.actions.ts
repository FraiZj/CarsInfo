import { Car } from '@cars/interfaces/car';
import { createAction, props } from "@ngrx/store";

export const fetchCarById = createAction(
  '[Car Details] Fetch Car By Id',
  props<{ id: number }>()
);

export const fetchCarByIdSuccess = createAction(
  '[Car Details] Fetch Car By Id Succcess',
  props<{ car: Car }>()
);

export const fetchCarByIdFailed = createAction(
  '[Car Details] Fetch Car By Id Failed'
);
