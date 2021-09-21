import {commentsListFeatureKey} from '../states/comments.state';
import {createFeatureSelector, createSelector} from "@ngrx/store";
import {commentFormFeatureKey } from "../states/comment-form.state";
import {commentsFeatureKey, CommentsState} from "../states";

export const selectCommentsState = createFeatureSelector<CommentsState>(commentsFeatureKey);

export const selectCommentsListState = createSelector(
  selectCommentsState,
  (state) => state[commentsListFeatureKey]
);

export const selectComments = createSelector(
  selectCommentsListState,
  (state) => state.comments
);

export const selectCanLoadNextComments = createSelector(
  selectCommentsListState,
  (state) => state.canLoadNext
);

export const selectCommentFilter = createSelector(
  selectCommentsListState,
  (state) => state.filter
);

export const selectCommentFormState = createSelector(
  selectCommentsState,
  (state) => state[commentFormFeatureKey]
);

export const selectCommentFormValidationErrors = createSelector(
  selectCommentFormState,
  (state) => state.validationErrors
);
