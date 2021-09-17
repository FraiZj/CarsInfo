import { CommentViewModel } from "../../interfaces/comment";


export const commentsFeatureKey = 'comments';

export interface CommentsState {
  comments: CommentViewModel[],
}

export interface State {
  [commentsFeatureKey]: CommentsState;
}