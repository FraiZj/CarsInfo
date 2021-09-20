import { CommentFilter } from './../../interfaces/comment-filter';
import { CommentEditor } from './../../interfaces/comment-editor';
import { createAction, props } from "@ngrx/store";
import { CommentViewModel } from '../../interfaces/comment';

export const fetchComments = createAction(
  '[Comments] Fetch Comments',
  props<{ carId: number, filter?: CommentFilter }>()
);

export const fetchCommentsSuccess = createAction(
  '[Comments] Fetch Comments Success',
  props<{ comments: CommentViewModel[] }>()
);

export const loadNextComments = createAction(
  '[Comments] Load Next Comments',
  props<{ carId: number, filter?: CommentFilter }>()
);

export const loadNextCommentsSuccess = createAction(
  '[Comments] Load Next Comments Success',
  props<{ comments: CommentViewModel[] }>()
);

export const addComment = createAction(
  '[Comments] Add Comment',
  props<{ carId: number, comment: CommentEditor }>()
);

export const addCommentSuccess = createAction(
  '[Comments] Add Comment Success',
  props<{ carId: number }>()
);

export const canLoadNextComments = createAction(
  '[Comments] Can Load Next Comments',
  props<{ can: boolean }>()
);

export const saveCommentsFilter = createAction(
  '[Comments] Save Comments Folter',
  props<{ filter: CommentFilter }>()
);
