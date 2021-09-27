export const appFeatureKey = 'app';

export interface AppState {
  applicationError: string | null;
}

export interface State {
  [appFeatureKey]: AppState;
}
