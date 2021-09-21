import {createFeatureSelector, createSelector} from "@ngrx/store";
import {coreFeatureKey, CoreState} from "@core/store/states/core.state";

export const selectCoreState = createFeatureSelector<CoreState>(coreFeatureKey);

export const selectApplicationError = createSelector(
  selectCoreState,
  (state) => state.applicationError
);
