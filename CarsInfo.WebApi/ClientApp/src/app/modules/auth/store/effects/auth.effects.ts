import {Router} from '@angular/router';
import {Injectable} from "@angular/core";
import {MatDialog} from "@angular/material/dialog";
import {AuthService} from "@auth/services/auth.service";
import {Actions, createEffect, ofType, OnInitEffects} from "@ngrx/effects";
import {of} from "rxjs";
import {map, exhaustMap, catchError, tap} from "rxjs/operators";
import * as AuthActions from '../actions/auth.actions';
import {AuthDialogComponent} from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import {Action} from '@ngrx/store';
import {AuthTokens} from "@auth/interfaces/auth-tokens";
import {handleError} from "@error-handler";

@Injectable()
export class AuthEffects implements OnInitEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly authService: AuthService,
    private readonly dialog: MatDialog,
    private readonly router: Router
  ) {
  }

  ngrxOnInitEffects(): Action {
    return AuthActions.init();
  }

  initLogin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.init),
      exhaustMap(() => {
          const oldTokens: AuthTokens | null = this.authService.getTokensFromLocalStorage();

          if (oldTokens == null) {
            return of(AuthActions.authTokenExpired());
          }

          return this.authService.refreshToken(oldTokens).pipe(
            map(tokens => AuthActions.loginSuccess({tokens})),
            catchError(() => {
              this.authService.removeTokensFromLocalStorage();
              return of(AuthActions.authTokenExpired());
            })
          )
        }
      )
    )
  );

  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      map(action => action.userRegister),
      exhaustMap(userRegister =>
        this.authService.register(userRegister).pipe(
          map(tokens => AuthActions.loginSuccess({tokens})),
          catchError(error => handleError(error))
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
          map(tokens => AuthActions.loginSuccess({tokens})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  refreshToken$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.refreshToken),
      map(action => action.tokens),
      exhaustMap(tokens => this.authService.refreshToken(tokens).pipe(
        map(tokens => AuthActions.loginSuccess({tokens})),
          catchError(() => of(AuthActions.logout()))
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
    {dispatch: false}
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      exhaustMap(() => this.authService.logout().pipe(
        map(() => {
          this.router.navigate(['cars']);
          return AuthActions.logoutSuccess();
        }))
      ))
  );

  loginWithGoogle$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginWithGoogle),
      map(action => action.token),
      exhaustMap(token =>
        this.authService.loginWithGoogle(token).pipe(
          map(tokens => AuthActions.loginSuccess({tokens})),
          catchError(error => handleError(error))
        )
      )
    )
  );
}
