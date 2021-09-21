import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CarsService } from "app/modules/cars/services/cars.service";
import {map, exhaustMap, catchError} from "rxjs/operators";
import {addCarCreationValidationErrors, createCar, createCarSuccess} from "../actions/car-creation.actions";
import {HttpErrorResponse} from "@angular/common/http";
import {ErrorResponse} from "@core/interfaces/error-response";
import {of} from "rxjs";
import {addApplicationError} from "@core/store/actions/core.actions";

@Injectable()
export class CarCreationEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    private readonly router: Router
  ) { }

  createCar$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createCar),
      map(action => action.car),
      exhaustMap((car) =>
        this.carsService.addCar(car).pipe(
          map(() => {
            this.router.navigateByUrl('/cars');
            return createCarSuccess();
          }),
          catchError(error => this.handleError(error))
        )
      )
    )
  );

  private handleError(error: Error) {
    if (error instanceof HttpErrorResponse) {
      const errorResponse: ErrorResponse = error.error;
      if (errorResponse.applicationError) {
        return of(addApplicationError({applicationError: errorResponse.applicationError}))
      }

      return of(addCarCreationValidationErrors({validationErrors: errorResponse.validationErrors}));
    }

    return of(addApplicationError({applicationError: error.message}))
  }
}
