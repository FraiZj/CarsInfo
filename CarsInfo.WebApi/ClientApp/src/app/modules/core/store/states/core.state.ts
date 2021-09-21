export const coreFeatureKey = 'core';

export interface CoreState {
  applicationError: string | null;
}

export interface State {
  [coreFeatureKey]: CoreState;
}
