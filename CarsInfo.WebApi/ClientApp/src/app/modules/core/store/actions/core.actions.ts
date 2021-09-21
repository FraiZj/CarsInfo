import {createAction, props} from "@ngrx/store";

export const addApplicationError = createAction(
  '[Core] Add Application Error',
  props<{ applicationError: string }>()
);

export const resetApplicationError = createAction(
  '[Core] Reset Application Error'
);
