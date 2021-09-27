import {Router} from '@angular/router';
import {fetchCarById, fetchCarByIdSuccess} from '../actions/car-details.actions';
import {CarsService} from '@cars/services/cars.service';
import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {catchError, exhaustMap, map, tap} from 'rxjs/operators';
import {handleError} from "@error-handler";

@Injectable()
export class CarDetailsEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    private readonly router: Router
  ) {
  }

  fetchCarById$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchCarById),
      map(action => action.id),
      exhaustMap(id =>
        this.carsService.getCarById(id).pipe(
          tap({
            error: () => this.router.navigateByUrl('not-found')
          }),
          map(car => fetchCarByIdSuccess({car})),
          catchError(error => handleError(error))
        )
      )
    )
  );
}
