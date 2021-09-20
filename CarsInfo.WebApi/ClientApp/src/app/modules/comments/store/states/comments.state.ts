import { CommentFilter } from './../../interfaces/comment-filter';
import { CommentViewModel } from "../../interfaces/comment";


export const commentsFeatureKey = 'comments';

export interface CommentsState {
  comments: CommentViewModel[],
  canLoadNext: boolean,
  filter: CommentFilter
}

export interface State {
  [commentsFeatureKey]: CommentsState;
}
