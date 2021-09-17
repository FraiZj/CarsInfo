import { CommentsState, commentsFeatureKey } from './../states/comments.state';
import { createFeatureSelector, createSelector } from "@ngrx/store";

export const selectCommentsState = createFeatureSelector<CommentsState>(commentsFeatureKey);

export const selectComments = createSelector(
  selectCommentsState,
  (state) => state.comments
);