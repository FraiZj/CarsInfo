import { fetchFilterBrands, fetchFilterBrandsSuccess } from './../actions/cars-brand-filter.actions';
import { BrandsService } from '@brands/services/brands.service';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { exhaustMap, map } from 'rxjs/operators';

@Injectable()
export class CarsBrandFilterEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly brandsService: BrandsService,
  ) { }

  fecthFilterCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchFilterBrands),
      map(action => action.brandName),
      exhaustMap(brandName =>
        this.brandsService.getBrands(brandName).pipe(
          map(brands => fetchFilterBrandsSuccess({ brands }))
        )
      )
    )
  );
}

