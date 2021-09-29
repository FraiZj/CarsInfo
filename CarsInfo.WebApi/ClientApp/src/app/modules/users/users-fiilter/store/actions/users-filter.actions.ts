import { UsersFilter } from '@users-shared';
import { createAction, props } from "@ngrx/store";

export const saveUsersFilter = createAction(
  '[Users Filter] Save Filter',
  props<{ filter: UsersFilter }>()
);