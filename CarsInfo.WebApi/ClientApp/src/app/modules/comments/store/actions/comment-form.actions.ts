import {createAction, props} from "@ngrx/store";
import {CommentEditor} from "../../interfaces/comment-editor";
import {ValidationError} from "@core/interfaces/error";

export const addComment = createAction(
  '[Comments] Add Comment',
  props<{ carId: number, comment: CommentEditor }>()
);

export const addCommentSuccess = createAction(
  '[Comments] Add Comment Success',
  props<{ carId: number }>()
);

export const addValidationErrors = createAction(
  '[Comments] Add Validation Errors',
  props<{ validationErrors: ValidationError[] }>()
);
