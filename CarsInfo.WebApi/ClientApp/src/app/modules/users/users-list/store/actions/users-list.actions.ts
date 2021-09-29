import { createAction, props } from "@ngrx/store";
import { User } from "@users-shared";

export const fetchUsers = createAction(
  '[Users List] Fetch Users'
);

export const fetchUsersSuccess = createAction(
  '[Users List] Fetch Users Success',
  props<{ users: User[] }>()
);