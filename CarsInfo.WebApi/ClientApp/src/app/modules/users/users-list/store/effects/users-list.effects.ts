import { fetchUsers, fetchUsersSuccess } from './../actions/users-list.actions';
import { UsersService } from '@users-shared';
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { handleError } from '@error-handler';
import { catchError, exhaustMap, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class UsersListEffects {

  constructor(
    private readonly actions$: Actions,
    private readonly usersService: UsersService
  ) {
  }

  fetchUsers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchUsers),
      exhaustMap(() =>
        this.usersService.getAll().pipe(
          map(users => fetchUsersSuccess({ users })),
          catchError(error => handleError(error))
        )
      )
    )
  );
}