import {createReducer, on} from "@ngrx/store";
import {CommentFormState} from "../states/comment-form.state";
import {addValidationErrors} from "../actions/comment-form.actions";

export const initialState: CommentFormState = {
  validationErrors: []
};

export const commentFormReducer = createReducer(
  initialState,
  on(addValidationErrors, (state, { validationErrors }) => ({ ...state, validationErrors })),
);
