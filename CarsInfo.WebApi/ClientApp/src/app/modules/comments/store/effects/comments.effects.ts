import { ItemsTakePerLoad } from '../../consts/comment-filter-consts';
import * as commentActions from '../actions/comments.actions';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CommentsService } from "../../services/comments.service"
import {catchError, exhaustMap, map} from 'rxjs/operators';
import {addComment, addCommentSuccess} from "../actions/comment-form.actions";
import { handleError } from '@error-handler';

@Injectable()
export class CommentsEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly commentsService: CommentsService
  ) { }

  fetchComments$ = createEffect(() =>
    this.actions$.pipe(
      ofType(commentActions.fetchComments),
      map(action => ({ carId: action.carId, filter: action.filter })),
      exhaustMap(({ carId, filter }) =>
        this.commentsService.getComments(carId, filter).pipe(
          map(comments => commentActions.fetchCommentsSuccess({ comments })),
          catchError(error => handleError(error))
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
          map(comments => commentActions.loadNextCommentsSuccess({ comments })),
          catchError(error => handleError(error))
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
      ofType(addComment),
      map(action => ({ carId: action.carId, comment: action.comment })),
      exhaustMap(({ carId, comment }) =>
        this.commentsService.addComment(carId, comment).pipe(
          map(() => addCommentSuccess({ carId })),
          catchError(error => handleError(error))
        )
      )
    )
  );

  fetchCarOnNewAdded$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addCommentSuccess),
      map(action => action.carId),
      map(carId => {
        return commentActions.fetchComments({ carId });
      })
    )
  );
}
