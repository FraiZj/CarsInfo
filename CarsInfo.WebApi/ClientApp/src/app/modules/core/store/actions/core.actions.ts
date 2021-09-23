import {createAction, props} from "@ngrx/store";

export const addApplicationError = createAction(
  '[Core] Add Application Error',
  props<{ applicationError: string }>()
);

export const resetApplicationError = createAction(
  '[Core] Reset Application Error'
);

export const sendVerificationEmail = createAction(
  '[Core] Send Email Verification'
);

export const sendVerificationEmailSuccess = createAction(
  '[Core] Send Email Verification Success'
);
