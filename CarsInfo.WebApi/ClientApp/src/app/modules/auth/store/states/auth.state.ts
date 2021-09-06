import { AuthTokens } from "@auth/interfaces/auth-tokens";

export const authFeatureKey = 'auth';

export interface AuthState {
  tokens: AuthTokens | null;
  error: string | null;
}

export interface State {
  [authFeatureKey]: AuthState;
}
