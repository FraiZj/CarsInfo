import { fetchCommentsSuccess } from './../actions/comments.actions';
import { createReducer, on } from '@ngrx/store';
import { CommentsState } from './../states/comments.state';

export const initialState: CommentsState = {
  comments: [],
};

export const reducer = createReducer(
  initialState,
  on(fetchCommentsSuccess, (state, { comments }) => ({ ...state, comments })),
);