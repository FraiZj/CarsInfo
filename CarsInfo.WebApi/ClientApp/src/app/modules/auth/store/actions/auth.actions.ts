import { UserRegister } from '@auth/interfaces/user-register';

import { UserLogin } from 'app/modules/auth/interfaces/user-login';
import { createAction, props } from '@ngrx/store';
import { AuthTokens } from '@auth/interfaces/auth-tokens';
import {ValidationError} from "@core/interfaces/error";

export const init = createAction('[Auth] Init');

export const initLogin = createAction(
  '[Auth] Init Login',
  props<{ userLogin: UserLogin }>()
);

export const authTokenExpired = createAction(
  '[Auth] Auth Token Expired'
);

export const register = createAction(
  '[Auth] Register',
  props<{ userRegister: UserRegister }>()
);

export const login = createAction(
  '[Auth] Login',
  props<{ userLogin: UserLogin }>()
);

export const refreshToken = createAction(
  '[Auth] Refresh Token',
  props<{ tokens: AuthTokens }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ tokens: AuthTokens }>()
);

export const loginRedirect = createAction(
  '[Auth] Login Redirect',
  props<{ returnUrl: string }>()
);

export const logout = createAction('[Auth] Logout');

export const logoutSuccess = createAction('[Auth] Logout Success');

export const clearLoginError = createAction('[Auth] Reset login error');

export const loginWithGoogle = createAction(
  '[Auth] Login With Google',
  props<{ token: string }>()
);

export const addAuthValidationErrors = createAction(
  '[Auth] Add Auth Validation Errors',
  props<{ validationErrors: ValidationError[] }>()
);
