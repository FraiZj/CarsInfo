import { AuthTokens } from './../../interfaces/auth-tokens';
import { ClaimTypes } from "@auth/enums/claim-types";
import { JwtPayload } from "@auth/interfaces/jwt-payload";
import { UserClaims } from "@auth/interfaces/user-claims";
import { createFeatureSelector, createSelector } from "@ngrx/store";
import jwtDecode from "jwt-decode";
import * as fromAuth from './../reducers/auth.reducer';

export interface AuthState {
  [fromAuth.authStatusFeatureKey]: fromAuth.State;
}

export const selectAuthStatusState = createFeatureSelector<fromAuth.State>(fromAuth.authStatusFeatureKey);

export const selectAuthTokens = createSelector(
  selectAuthStatusState,
  (state) => state.tokens
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
  const userClaims: UserClaims = {
    roles: getRoles(jwtPayload),
    id: +jwtPayload.Id,
    email: jwtPayload[ClaimTypes.Email],
    token: tokens.accessToken
  };

  return userClaims;
}

function getRoles(jwtPayload: JwtPayload) {
  return typeof jwtPayload[ClaimTypes.Role] == 'string' ?
    [jwtPayload[ClaimTypes.Role] as string] :
    jwtPayload[ClaimTypes.Role] as string[];
}
