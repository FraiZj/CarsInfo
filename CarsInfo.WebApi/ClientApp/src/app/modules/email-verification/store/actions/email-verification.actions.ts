import {createAction, props} from "@ngrx/store";

export const verifyEmail = createAction(
  '[Email Verification] Verify Email',
  props<{ token: string }>()
);

export const verifyEmailSuccess = createAction(
  '[Email Verification] Verify Email Success'
);
