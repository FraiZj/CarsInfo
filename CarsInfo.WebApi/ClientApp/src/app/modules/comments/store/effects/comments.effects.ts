import { ItemsSkipPerLoad } from './../../consts/comment-filter-consts';
import { CommentFilter } from './../../interfaces/comment-filter';
import { selectCommentFilter } from './../selectors/comments.selectors';
import { Store } from '@ngrx/store';
import { ItemsTakePerLoad } from './../../consts/comment-filter-consts';
import * as commentActions from './../actions/comments.actions';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CommentsService } from "../../services/comments.service"
import { exhaustMap, map } from 'rxjs/operators';
import { CommentOrderBy } from '../../enums/comment-order-by';

@Injectable()
export class CommentsEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly commentsService: CommentsService,
    private readonly store: Store
  ) { }

  fetchComments$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.fetchComments),
      map(action => ({ carId: action.carId, filter: action.filter })),
      exhaustMap(({ carId, filter }) =>
        this.commentsService.getComments(carId, filter).pipe(
          map(comments => commentActions.fetchCommentsSuccess({ comments }))
        )
      )
    )
  );

  fetchCommentsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.fetchCommentsSuccess),
      map(() => commentActions.canLoadNextComments({ can: true }))
    )
  );

  loadNextComments$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.loadNextComments),
      map(action => ({ carId: action.carId, filter: action.filter })),
      exhaustMap(({ carId, filter }) =>
        this.commentsService.getComments(carId, filter).pipe(
          map(comments => commentActions.loadNextCommentsSuccess({ comments }))
        )
      )
    )
  );

  loadNextCarsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.loadNextCommentsSuccess),
      map(action => action.comments),
      map(comments => commentActions.canLoadNextComments({ can: comments.length == ItemsTakePerLoad }))
    )
  );

  addComment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.addComment),
      map(action => ({ carId: action.carId, comment: action.comment })),
      exhaustMap(({ carId, comment }) =>
        this.commentsService.addComment(carId, comment).pipe(
          map(() => commentActions.addCommentSuccess({ carId }))
        )
      )
    )
  );

  fetchCarOnNewAdded$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.addCommentSuccess),
      map(action => action.carId),
      map(carId => {
        return commentActions.fetchComments({ carId });
      })
    )
  );
}
