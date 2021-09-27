import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CarsService } from "app/modules/cars/services/cars.service";
import {map, exhaustMap, catchError} from "rxjs/operators";
import {createCar, createCarSuccess} from "../actions/car-creation.actions";
import {handleError} from "@error-handler";

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
          catchError(error => handleError(error))
        )
      )
    )
  );
}
