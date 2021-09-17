import { fetchCommentsSuccess, addComment, addCommentSuccess } from './../actions/comments.actions';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CommentsService } from "../../services/comments.service"
import { fetchComments } from "../actions/comments.actions";
import { exhaustMap, map } from 'rxjs/operators';

@Injectable()
export class CommentsEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly commentsService: CommentsService,
  ) { }

  fetchComments$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchComments),
      map(action => action.carId),
      exhaustMap(carId =>
        this.commentsService.getComments(carId).pipe(
          map(comments => fetchCommentsSuccess({ comments }))
        )
      )
    )
  );

  addComment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addComment),
      map(action => ({ carId: action.carId, comment: action.comment })),
      exhaustMap(({ carId, comment }) =>
        this.commentsService.addComment(carId, comment).pipe(
          map(() => addCommentSuccess())
        )
      )
    )
  );
}