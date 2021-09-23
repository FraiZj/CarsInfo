import {Injectable} from "@angular/core";
import {Actions} from "@ngrx/effects";
import {AccountService} from "@account/services/account.service";
import {HttpErrorResponse} from "@angular/common/http";
import {of} from "rxjs";
import {addApplicationError} from "@core/store/actions/core.actions";

@Injectable()
export class EmailVerificationEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly accountService: AccountService
  ) {
  }

  private handleError(error: Error) {
    if (error instanceof HttpErrorResponse && error.error.applicationError) {
      return of(addApplicationError({applicationError: error.error.applicationError}))
    }

    return of(addApplicationError({applicationError: error.message}))
  }
}
