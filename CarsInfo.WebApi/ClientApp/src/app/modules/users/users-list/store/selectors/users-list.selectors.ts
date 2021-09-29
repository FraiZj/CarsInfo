import { UsersListState, usersListFeatureKey } from './../states/users-list.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCarsListState = createFeatureSelector<UsersListState>(usersListFeatureKey);

export const selectUsers = createSelector(
  selectCarsListState,
  (state) => state.users
);
