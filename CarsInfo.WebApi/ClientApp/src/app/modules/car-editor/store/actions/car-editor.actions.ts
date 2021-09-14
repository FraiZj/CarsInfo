import { CarEditor } from '@cars/interfaces/car-editor';
import { createAction, props } from "@ngrx/store";

export const fetchCarEditorById = createAction(
  '[Car Editor] Fetch Car Editor By Id',
  props<{ id: number }>()
);

export const fetchCarEditorByIdSuccess = createAction(
  '[Car Editor] Fetch Car Editor By Id Succcess',
  props<{ carEditor: CarEditor }>()
);

export const updateCar = createAction(
  '[Car Editor] Update Car',
  props<{ id: number, carEditor: CarEditor }>()
);

export const updateCarSuccess = createAction(
  '[Car Editor] Update Car Success'
);
