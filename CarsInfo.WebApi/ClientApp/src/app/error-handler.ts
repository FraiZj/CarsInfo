import {HttpErrorResponse} from "@angular/common/http";
import {addApplicationError} from "./store/actions/app.actions";
import {of} from "rxjs";

export const handleError = (error: Error) => {
  if (!(error instanceof HttpErrorResponse)) {
    return of(addApplicationError({applicationError: error.message}))
  }

  switch (error.status) {
    case 400 : return of(addApplicationError({applicationError: error.error.applicationError}));
    case 401: return of(addApplicationError({applicationError: 'You are not authorized'}));
    case 404: return of(addApplicationError({applicationError: 'Resource not found'}));
  }

  return of(addApplicationError({applicationError: 'An error occurred'}));
}
