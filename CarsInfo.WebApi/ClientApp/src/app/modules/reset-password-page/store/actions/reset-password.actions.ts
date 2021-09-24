import {createAction, props} from "@ngrx/store";
import {ResetPasswordPayload} from "@account/interfaces/reset-password-payload";

export const resetPassword = createAction(
  '[Reset Password] Reset Password',
  props<{ payload: ResetPasswordPayload }>()
);

export const verifyEmailSuccess = createAction(
  '[Reset Password] Reset Password Success'
);

export const sendResetPassword = createAction(
  '[Reset Password] Send Reset Password',
  props<{ email: string }>()
);

export const sendResetPasswordSuccess = createAction(
  '[Reset Password] Send Reset Password Success'
);
