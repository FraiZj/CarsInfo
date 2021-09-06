import { createReducer, on } from '@ngrx/store';
import * as AuthActions from '@auth/store/actions/auth.actions';
import { AuthState } from '../states/auth.state';

export const initialState: AuthState = {
  tokens: null,
  error: null
};

export const reducer = createReducer(
  initialState,
  on(AuthActions.loginSuccess, (state, { tokens }) => ({ ...state, tokens, error: null })),
  on(AuthActions.loginFailure, (state, { error }) => ({ ...state, tokens: null, error })),
  on(AuthActions.logoutSuccess, (state) => ({ ...state, tokens: null, error: null })),
  on(AuthActions.clearLoginError, (state) => ({ ...state, error: null }))
);

export function authStatusFeatureKey<T>(authStatusFeatureKey: any) {
  throw new Error('Function not implemented.');
}

