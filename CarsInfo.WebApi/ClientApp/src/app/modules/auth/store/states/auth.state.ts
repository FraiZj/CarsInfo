import { AuthTokens } from "@auth/interfaces/auth-tokens";
import {ValidationError} from "@core/interfaces/error";

export const authFeatureKey = 'auth';

export interface AuthState {
  tokens: AuthTokens | null;
  validationErrors: ValidationError[];
}

export interface State {
  [authFeatureKey]: AuthState;
}
