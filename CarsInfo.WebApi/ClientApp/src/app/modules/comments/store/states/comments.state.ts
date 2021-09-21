import { CommentFilter } from '../../interfaces/comment-filter';
import { CommentViewModel } from "../../interfaces/comment";

export const commentsListFeatureKey = 'commentsList';

export interface CommentsListState {
  comments: CommentViewModel[],
  canLoadNext: boolean,
  filter: CommentFilter
}


