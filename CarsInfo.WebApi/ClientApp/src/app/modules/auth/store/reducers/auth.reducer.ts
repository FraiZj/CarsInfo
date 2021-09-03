import { AuthTokens } from '@auth/interfaces/auth-tokens';
import { createReducer, on } from '@ngrx/store';
import * as AuthActions from '@auth/store/actions/auth.actions';

export const authStatusFeatureKey = 'status';

export interface State {
  tokens: AuthTokens | null;
}

export const initialState: State = {
  tokens: null
};

export const reducers = createReducer(
  initialState,
  on(AuthActions.loginSuccess, (state, { tokens }) => ({ ...state, tokens })),
  on(AuthActions.logoutSuccess, (state) => ({ ...state, tokens: null }))
);
