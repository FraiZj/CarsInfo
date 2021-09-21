import {
  fetchCommentsSuccess,
  loadNextCommentsSuccess,
  canLoadNextComments,
  loadNextComments,
  saveCommentsFilter
} from '../actions/comments.actions';
import {createReducer, on} from '@ngrx/store';
import {CommentOrderBy} from '../../enums/comment-order-by';
import {CommentsListState} from "../states/comments.state";

export const initialState: CommentsListState = {
  comments: [],
  canLoadNext: true,
  filter: {
    skip: 0,
    take: 10,
    orderBy: CommentOrderBy.PublishDateDesc
  }
};

export const commentsListReducer = createReducer(
  initialState,
  on(fetchCommentsSuccess, (state, {comments}) => ({...state, comments})),
  on(loadNextComments, (state, {filter}) => ({...state, filter: filter ?? state.filter})),
  on(loadNextCommentsSuccess, (state, {comments}) => ({...state, comments: state.comments.concat(comments)})),
  on(canLoadNextComments, (state, {can}) => ({...state, canLoadNext: can})),
  on(saveCommentsFilter, (state, {filter}) => ({...state, filter}))
);
