import { CarEditor } from '@cars/interfaces/car-editor';
import { createAction, props } from "@ngrx/store";
import {ValidationError} from "@core/interfaces/error";

export const fetchCarEditorById = createAction(
  '[Car Editor] Fetch Car Editor By Id',
  props<{ id: number }>()
);

export const fetchCarEditorByIdSuccess = createAction(
  '[Car Editor] Fetch Car Editor By Id Success',
  props<{ carEditor: CarEditor }>()
);

export const updateCar = createAction(
  '[Car Editor] Update Car',
  props<{ id: number, carEditor: CarEditor }>()
);

export const updateCarSuccess = createAction(
  '[Car Editor] Update Car Success'
);

export const addCarEditorValidationErrors = createAction(
  '[Car Editor] Add Validation Errors',
  props<{ validationErrors: ValidationError[] }>()
);
