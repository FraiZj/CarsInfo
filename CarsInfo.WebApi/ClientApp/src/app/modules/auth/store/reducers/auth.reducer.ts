import {createReducer, on} from '@ngrx/store';
import * as AuthActions from '@auth/store/actions/auth.actions';
import {AuthState} from '../states/auth.state';

export const initialState: AuthState = {
  tokens: null,
  validationErrors: []
};

export const reducer = createReducer(
  initialState,
  on(AuthActions.loginSuccess, (state, {tokens}) => ({...state, tokens, errors: []})),
  on(AuthActions.addAuthValidationErrors, (state, {validationErrors}) => ({...state, tokens: null, validationErrors})),
  on(AuthActions.logoutSuccess, (state) => ({...state, tokens: null, errors: []})),
  on(AuthActions.clearLoginError, (state) => ({...state, errors: []}))
);
