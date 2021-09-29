import { saveUsersFilter } from './../actions/users-filter.actions';
import { createReducer, on } from "@ngrx/store";
import { UsersFilterState } from "../states/users-filter.state";

export const initialState: UsersFilterState = {
  filter: null
};

export const usersFilterReducer = createReducer(
  initialState,
  on(saveUsersFilter, (state, { filter }) => ({ ...state, filter })),
);