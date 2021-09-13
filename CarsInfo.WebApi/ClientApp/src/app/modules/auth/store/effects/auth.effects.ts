import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { AuthService } from "@auth/services/auth.service";
import { Actions, createEffect, ofType, OnInitEffects } from "@ngrx/effects";
import { of } from "rxjs";
import { map, exhaustMap, catchError, tap } from "rxjs/operators";
import * as AuthActions from '../actions/auth.actions';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import { Action } from '@ngrx/store';

@Injectable()
export class AuthEffects implements OnInitEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly authService: AuthService,
    private readonly dialog: MatDialog
  ) { }

  ngrxOnInitEffects(): Action {
    return AuthActions.init();
  }

  initLogin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.init),
      exhaustMap(() =>
        this.authService.refreshToken().pipe(
          map(tokens => AuthActions.loginSuccess({ tokens })),
          catchError(error => this.handleCatchError(error))
        )
      )
    )
  );

  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      map(action => action.userRegister),
      exhaustMap(userRegister =>
        this.authService.register(userRegister).pipe(
          map(tokens => AuthActions.loginSuccess({ tokens })),
          catchError(error => this.handleCatchError(error))
        )
      )
    )
  );

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      map(action => action.userLogin),
      exhaustMap(userLogin =>
        this.authService.login(userLogin).pipe(
          map(tokens => AuthActions.loginSuccess({ tokens })),
          catchError(error => this.handleCatchError(error))
        )
      )
    )
  );

  loginRedirect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginRedirect),
      map(action => action.returnUrl),
      tap((returnUrl) => {
        this.dialog.open(AuthDialogComponent, {
          data: {
            form: 'Login',
            returnUrl: returnUrl
          }
        });
      })
    ),
    { dispatch: false }
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      exhaustMap(() => this.authService.logout().pipe(
        map(() => AuthActions.logoutSuccess()))
      ))
  );

  private handleCatchError(error: any) {
    if (error instanceof HttpErrorResponse) return of(AuthActions.loginFailure({ error: error.error }));
    if (error instanceof Error) return of(AuthActions.loginFailure({ error: error.message }));
    return of(AuthActions.loginFailure({ error: 'An error occurred' }));
  }
}
