import { UsersFilterState, usersFilterFeatureKey } from './../states/users-filter.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectUsersFilterState = createFeatureSelector<UsersFilterState>(usersFilterFeatureKey);

export const selectFilter = createSelector(
  selectUsersFilterState,
  (state) => state.filter
);