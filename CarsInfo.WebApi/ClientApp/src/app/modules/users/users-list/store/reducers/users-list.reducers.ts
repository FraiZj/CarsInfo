import { fetchUsersSuccess } from './../actions/users-list.actions';
import { createReducer, on } from '@ngrx/store';
import { UsersListState } from './../states/users-list.state';

export const initialState: UsersListState = {
  users: []
};

export const usersListreducer = createReducer(
  initialState,
  on(fetchUsersSuccess, (state, { users }) => ({ ...state, users })),
);