import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {AccountService} from "@account/services/account.service";
import {HttpErrorResponse} from "@angular/common/http";
import {of} from "rxjs";
import {addApplicationError} from "@core/store/actions/core.actions";
import {catchError, exhaustMap, filter, map, tap} from "rxjs/operators";
import {verifyEmail, verifyEmailSuccess} from "../actions/email-verification.actions";
import {init, refreshToken} from "@auth/store/actions/auth.actions";
import {SnackBarService} from "@core/services/snackbar.service";
import {Router} from "@angular/router";
import {Store} from "@ngrx/store";
import {selectAuthTokens} from "@auth/store/selectors/auth.selectors";

@Injectable()
export class EmailVerificationEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly accountService: AccountService,
    private readonly snackBar: SnackBarService,
    private readonly router: Router,
    private readonly store: Store
  ) {
  }

  verifyEmail$ = createEffect(() =>
    this.actions$.pipe(
      ofType(verifyEmail),
      map(action => action.token),
      exhaustMap(token => this.accountService.verifyEmail(token).pipe(
          map(() => verifyEmailSuccess()),
          catchError(() => {
            this.router.navigateByUrl('/cars');
            return this.handleError(new Error('Invalid token. Login to account and resend verification email.'));
          })
        )
      )
    )
  );

  verifyEmailSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(verifyEmailSuccess),
      tap(() => {
        this.snackBar.success('Email successfully verified.');
        this.router.navigateByUrl('/cars');
      }),
      exhaustMap(() => this.store.select(selectAuthTokens).pipe(
        filter(tokens => tokens != null),
        map(tokens => refreshToken({ tokens: tokens! }))
      ))
    )
  );

  private handleError(error: Error) {
    if (error instanceof HttpErrorResponse && error.error.applicationError) {
      return of(addApplicationError({applicationError: error.error.applicationError}))
    }

    return of(addApplicationError({applicationError: error.message}))
  }
}