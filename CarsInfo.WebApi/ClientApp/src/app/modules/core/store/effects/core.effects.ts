import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {catchError, exhaustMap, map} from "rxjs/operators";
import {AccountService} from "@account/services/account.service";
import {
  sendVerificationEmail,
  sendVerificationEmailSuccess
} from "@core/store/actions/core.actions";
import {handleError} from "@error-handler";
import {SnackBarService} from "@core/services/snackbar.service";

@Injectable()
export class CoreEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly accountService: AccountService,
    private readonly snackBar: SnackBarService
  ) {
  }

  sendVerificationEmail$ = createEffect(() =>
    this.actions$.pipe(
      ofType(sendVerificationEmail),
      exhaustMap(() => this.accountService.sendVerificationEmail().pipe(
          map(() => {
            this.snackBar.success('Verification email sent to your email box');
            return sendVerificationEmailSuccess();
          }),
          catchError(error => handleError(error))
        )
      )
    )
  );
}
