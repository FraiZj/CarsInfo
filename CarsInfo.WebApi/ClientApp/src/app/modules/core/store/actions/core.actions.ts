import {createAction} from "@ngrx/store";

export const sendVerificationEmail = createAction(
  '[Core] Send Email Verification'
);

export const sendVerificationEmailSuccess = createAction(
  '[Core] Send Email Verification Success'
);
