import {HttpErrorResponse} from "@angular/common/http";
import {addApplicationError} from "./store/actions/app.actions";
import {of} from "rxjs";

export const handleError = (error: Error) => {
  if (error instanceof HttpErrorResponse) {
    return of(addApplicationError({applicationError: error.error.applicationError}))
  }

  return of(addApplicationError({applicationError: error.message}))
}
