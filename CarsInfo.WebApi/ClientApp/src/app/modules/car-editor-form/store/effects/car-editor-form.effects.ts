import { fetchBrands, fetchBrandsSuccess, createBrand } from '../actions/car-editor-form.actions';
import { BrandsService } from '@brands/services/brands.service';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {catchError, exhaustMap, map} from 'rxjs/operators';
import {handleError} from "@error-handler";

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
          catchError(error => handleError(error))
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
          catchError(error => handleError(error))
        )
      )
    )
  );
}

