import {createAction, props} from "@ngrx/store";

export const deleteCarById = createAction(
  '[Car Deletion] Delete Car By Id',
  props<{ id: number }>()
);

export const deleteCarByIdSuccess = createAction(
  '[Car Deletion] Delete Car By Id Success'
);
