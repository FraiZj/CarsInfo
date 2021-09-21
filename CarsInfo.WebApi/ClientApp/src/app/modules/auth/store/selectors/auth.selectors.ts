import {AuthTokens} from '@auth/interfaces/auth-tokens';
import {ClaimTypes} from "@auth/enums/claim-types";
import {JwtPayload} from "@auth/interfaces/jwt-payload";
import {UserClaims} from "@auth/interfaces/user-claims";
import {createFeatureSelector, createSelector} from "@ngrx/store";
import jwtDecode from "jwt-decode";
import * as fromAuth from './../states/auth.state';
import {AuthState} from './../states/auth.state';

export const selectAuthState = createFeatureSelector<AuthState>(fromAuth.authFeatureKey);

export const selectAuthTokens = createSelector(
  selectAuthState,
  (state) => state.tokens
);

export const selectAuthValidationErrors = createSelector(
  selectAuthState,
  (state) => state.validationErrors
);

export const selectUserClaims = createSelector(
  selectAuthTokens,
  (tokens) => getCurrentUserClaims(tokens)
);

export const selectLoggedIn = createSelector(selectAuthTokens, (tokens) => !!tokens);

function getCurrentUserClaims(tokens: AuthTokens | null): UserClaims | null {
  if (tokens == null) {
    return null;
  }

  const jwtPayload = jwtDecode<JwtPayload>(tokens.accessToken);
  return {
    roles: getRoles(jwtPayload),
    id: +jwtPayload.Id,
    email: jwtPayload[ClaimTypes.Email],
    token: tokens.accessToken
  };
}

function getRoles(jwtPayload: JwtPayload) {
  return typeof jwtPayload[ClaimTypes.Role] == 'string' ?
    [jwtPayload[ClaimTypes.Role] as string] :
    jwtPayload[ClaimTypes.Role] as string[];
}
