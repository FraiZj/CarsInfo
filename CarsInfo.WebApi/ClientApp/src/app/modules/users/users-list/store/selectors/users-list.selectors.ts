import { UsersListState, usersListFeatureKey } from './../states/users-list.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectUsersListState = createFeatureSelector<UsersListState>(usersListFeatureKey);

export const selectUsers = createSelector(
  selectUsersListState,
  (state) => state.users
);
