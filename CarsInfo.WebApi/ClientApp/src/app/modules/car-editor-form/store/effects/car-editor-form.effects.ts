import { fetchBrands, fetchBrandsSuccess, createBrand } from '../actions/car-editor-form.actions';
import { BrandsService } from '@brands/services/brands.service';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {catchError, exhaustMap, map} from 'rxjs/operators';
import {HttpErrorResponse} from "@angular/common/http";
import {of} from "rxjs";
import {addApplicationError} from "@core/store/actions/core.actions";
import {ErrorResponse} from "@core/interfaces/error-response";

@Injectable()
export class CarEditorFormEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly brandsService: BrandsService,
  ) { }

  fetchBrands$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchBrands),
      exhaustMap(() =>
        this.brandsService.getBrands().pipe(
          map(brands => fetchBrandsSuccess({ brands })),
          catchError(error => this.handleError(error))
        )
      )
    )
  );

  createBrand$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createBrand),
      map(action => action.brand),
      exhaustMap(brand =>
        this.brandsService.addBrand(brand).pipe(
          map(() => fetchBrands()),
          catchError(error => this.handleError(error))
        )
      )
    )
  );

  private handleError(error: Error ) {
    if (error instanceof HttpErrorResponse) {
      const errorResponse: ErrorResponse = error.error;
      if (errorResponse.applicationError) {
        return of(addApplicationError({ applicationError: errorResponse.applicationError }))
      }

      return of(addApplicationError({ applicationError: errorResponse.validationErrors[0].error }));
    }

    return of(addApplicationError({ applicationError: error.message }))
  }
}

