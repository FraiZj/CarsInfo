import { User } from "@users-shared";

export const usersListFeatureKey = 'usersList';

export interface UsersListState {
  users: User[];
}

export interface State {
  [usersListFeatureKey]: UsersListState;
}