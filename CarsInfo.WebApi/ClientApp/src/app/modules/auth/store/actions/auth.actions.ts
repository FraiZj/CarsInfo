import { UserRegister } from './../../interfaces/user-register';

import { UserLogin } from 'app/modules/auth/interfaces/user-login';
import { createAction, props } from '@ngrx/store';
import { AuthTokens } from '@auth/interfaces/auth-tokens';

export const register = createAction(
  '[Auth] Register',
  props<{ userRegister: UserRegister }>()
);

export const login = createAction(
  '[Auth] Login',
  props<{ userLogin: UserLogin }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ tokens: AuthTokens }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: any }>()
);

export const loginRedirect = createAction(
  '[Auth] Login Redirect',
  props<{ returnUrl: string }>()
);

export const logout = createAction('[Auth] Logout');

export const logoutSuccess = createAction('[Auth] Logout Success');


