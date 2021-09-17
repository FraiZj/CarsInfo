import { CommentEditor } from './../../interfaces/comment-editor';
import { createAction, props } from "@ngrx/store";
import { CommentViewModel } from '../../interfaces/comment';

export const fetchComments = createAction(
  '[Comments] Fetch Comments',
  props<{ carId: number }>()
);

export const fetchCommentsSuccess = createAction(
  '[Comments] Fetch Comments Success',
  props<{ comments: CommentViewModel[] }>()
);

export const addComment = createAction(
  '[Comments] Add Comment',
  props<{ carId: number, comment: CommentEditor }>()
);

export const addCommentSuccess = createAction(
  '[Comments] Add Comment Success'
);