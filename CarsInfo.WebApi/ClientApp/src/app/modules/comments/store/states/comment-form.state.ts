import {ValidationError} from "@core/interfaces/error";

export const commentFormFeatureKey = 'commentForm';

export interface CommentFormState {
  validationErrors: ValidationError[];
}
