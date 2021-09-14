import { fetchCarEditorById, fetchCarEditorByIdSuccess, updateCar, updateCarSuccess } from './../actions/car-editor.actions';
import { Router } from '@angular/router';
import { CarsService } from '@cars/services/cars.service';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { exhaustMap, map, tap } from 'rxjs/operators';

@Injectable()
export class CarEditorEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    private readonly router: Router
  ) { }

  fecthCarById$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchCarEditorById),
      map(action => action.id),
      exhaustMap(id =>
        this.carsService.getCarEditorById(id).pipe(
          tap({
            error: () => this.router.navigateByUrl('not-found')
          }),
          map(carEditor => fetchCarEditorByIdSuccess({ carEditor })),
        )
      )
    )
  );

  updateBrand$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateCar),
      map(action => ({ id: action.id, car: action.carEditor })),
      exhaustMap(({ id, car }) =>
        this.carsService.updateCar(id, car).pipe(
          map(() => {
            this.router.navigateByUrl(`/cars/${id}`);
            return updateCarSuccess();
          })
        )
      )
    )
  );
}
