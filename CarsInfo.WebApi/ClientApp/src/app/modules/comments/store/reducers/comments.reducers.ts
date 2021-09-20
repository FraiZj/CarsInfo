import { fetchCommentsSuccess, loadNextCommentsSuccess, canLoadNextComments, loadNextComments, saveCommentsFilter } from './../actions/comments.actions';
import { createReducer, on } from '@ngrx/store';
import { CommentsState } from './../states/comments.state';
import { CommentOrderBy } from '../../enums/comment-order-by';

export const initialState: CommentsState = {
  comments: [],
  canLoadNext: true,
  filter: {
    skip: 0,
    take: 10,
    orderBy: CommentOrderBy.PublishDateDesc
  }
};

export const reducer = createReducer(
  initialState,
  on(fetchCommentsSuccess, (state, { comments }) => ({ ...state, comments })),
  on(loadNextComments, (state, { filter }) => ({ ...state, filter: filter ?? state.filter })),
  on(loadNextCommentsSuccess, (state, { comments }) => ({ ...state, comments: state.comments.concat(comments) })),
  on(canLoadNextComments, (state, { can }) => ({ ...state, canLoadNext: can })),
  on(saveCommentsFilter, (state, { filter }) => ({ ...state, filter }))
);
