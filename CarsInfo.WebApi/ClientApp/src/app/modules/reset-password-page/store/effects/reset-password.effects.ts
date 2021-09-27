import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {AccountService} from "@account/services/account.service";
import {catchError, exhaustMap, map} from "rxjs/operators";
import {SnackBarService} from "@core/services/snackbar.service";
import {Router} from "@angular/router";
import {resetPassword, sendResetPassword, sendResetPasswordSuccess} from "../actions/reset-password.actions";
import {handleError} from "@error-handler";

@Injectable()
export class ResetPasswordEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly accountService: AccountService,
    private readonly snackBar: SnackBarService,
    private readonly router: Router
  ) {
  }

  sendResetPassword$ = createEffect(() =>
    this.actions$.pipe(
      ofType(sendResetPassword),
      map(action => action.email),
      exhaustMap(email => {
          return this.accountService.sendResetPasswordEmail(email).pipe(
            map(() => {
              this.snackBar.success('Reset password email sent.');
              this.router.navigateByUrl('/cars');
              return sendResetPasswordSuccess();
            }),
            catchError(error => handleError(error))
          );
        }
      )
    )
  );

  resetPassword$ = createEffect(() =>
    this.actions$.pipe(
      ofType(resetPassword),
      map(action => action.payload),
      exhaustMap(payload => this.accountService.resetPassword(payload).pipe(
          map(() => {
            this.snackBar.success('Password updated.');
            this.router.navigateByUrl('/cars');
            return sendResetPasswordSuccess();
          }),
          catchError(error => {
            this.router.navigateByUrl('/cars');
            return handleError(error);
          })
        )
      )
    )
  );


}
