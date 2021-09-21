import {commentsListFeatureKey, CommentsListState} from "./comments.state";
import {commentFormFeatureKey, CommentFormState} from "./comment-form.state";

export const commentsFeatureKey = 'comments';

export interface CommentsState {
  [commentsListFeatureKey]: CommentsListState;
  [commentFormFeatureKey]: CommentFormState;
}

export interface State {
  [commentsFeatureKey]: CommentsState
}
