import { AuthTokens } from "@auth/interfaces/auth-tokens";

export const authFeatureKey = 'auth';

export interface AuthState {
  tokens: AuthTokens | null;
  errors: string[];
}

export interface State {
  [authFeatureKey]: AuthState;
}
