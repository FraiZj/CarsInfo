import { CommentOrderBy } from './../enums/comment-order-by';
export interface CommentFilter {
  skip: number;
  take: number;
  orderBy: CommentOrderBy;
}
