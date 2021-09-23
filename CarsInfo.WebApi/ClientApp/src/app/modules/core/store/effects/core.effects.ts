import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {catchError, exhaustMap, map} from "rxjs/operators";
import {AccountService} from "@account/services/account.service";
import {HttpErrorResponse} from "@angular/common/http";
import {of} from "rxjs";
import {
  addApplicationError,
  sendVerificationEmail,
  sendVerificationEmailSuccess
} from "@core/store/actions/core.actions";

@Injectable()
export class CoreEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly accountService: AccountService
  ) {
  }

  sendVerificationEmail$ = createEffect(() =>
    this.actions$.pipe(
      ofType(sendVerificationEmail),
      exhaustMap(() => this.accountService.sendVerificationEmail().pipe(
          map(() => sendVerificationEmailSuccess()),
          catchError(error => this.handleError(error))
        )
      )
    )
  );

  private handleError(error: Error) {
    if (error instanceof HttpErrorResponse && error.error.applicationError) {
      return of(addApplicationError({applicationError: error.error.applicationError}))
    }

    return of(addApplicationError({applicationError: error.message}))
  }
}
