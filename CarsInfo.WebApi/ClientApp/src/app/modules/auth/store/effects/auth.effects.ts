import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { AuthService } from "@auth/services/auth.service";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { map, exhaustMap, catchError, tap } from "rxjs/operators";
import * as AuthActions from '../actions/auth.actions';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';

@Injectable()
export class AuthEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly dialog: MatDialog
  ) { }

  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      map(action => action.userRegister),
      exhaustMap(userRegister =>
        this.authService.register(userRegister).pipe(
          map(tokens => AuthActions.loginSuccess({ tokens })),
          catchError(error => of(AuthActions.loginFailure({ error })))
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
          catchError(error => of(AuthActions.loginFailure({ error })))
        )
      )
    )
  );

  loginSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginSuccess),
      tap(() => this.router.navigate(['/cars']))
    ),
    { dispatch: false }
  );

  loginRedirect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginRedirect),
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
}
