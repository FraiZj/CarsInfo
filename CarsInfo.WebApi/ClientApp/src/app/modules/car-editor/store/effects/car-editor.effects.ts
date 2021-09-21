import {
  addCarEditorValidationErrors,
  fetchCarEditorById,
  fetchCarEditorByIdSuccess,
  updateCar,
  updateCarSuccess
} from '../actions/car-editor.actions';
import {Router} from '@angular/router';
import {CarsService} from '@cars/services/cars.service';
import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {catchError, exhaustMap, map, tap} from 'rxjs/operators';
import {HttpErrorResponse} from "@angular/common/http";
import {ErrorResponse} from "@core/interfaces/error-response";
import {of} from "rxjs";
import {addApplicationError} from "@core/store/actions/core.actions";

@Injectable()
export class CarEditorEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    private readonly router: Router
  ) {
  }

  fetchCarById$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchCarEditorById),
      map(action => action.id),
      exhaustMap(id =>
        this.carsService.getCarEditorById(id).pipe(
          tap({
            error: () => this.router.navigateByUrl('not-found')
          }),
          map(carEditor => fetchCarEditorByIdSuccess({carEditor})),
          catchError(error => this.handleError(error))
        )
      )
    )
  );

  updateBrand$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateCar),
      map(action => ({id: action.id, car: action.carEditor})),
      exhaustMap(({id, car}) =>
        this.carsService.updateCar(id, car).pipe(
          map(() => {
            this.router.navigateByUrl(`/cars/${id}`);
            return updateCarSuccess();
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

      return of(addCarEditorValidationErrors({validationErrors: errorResponse.validationErrors}));
    }

    return of(addApplicationError({applicationError: error.message}))
  }
}
