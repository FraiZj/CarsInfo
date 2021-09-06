import { CarsService } from '@cars/services/cars.service';
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as CarsListActions from './../actions/cars-list.actions'
import { exhaustMap, map } from 'rxjs/operators';
import { ItemsTakePerLoad } from '@cars-list/consts/filter-consts';

@Injectable()
export class CarsListEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
  ) { }

  fetchCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.fetchCars),
      map(action => ({ filter: action.filter, orderBy: action.orderBy })),
      exhaustMap(({ filter, orderBy }) =>
        this.carsService.getCars(filter, orderBy).pipe(
          map(cars => CarsListActions.fetchCarsSuccess({ cars }))
        )
      )
    )
  );

  loadNextCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextCars),
      map(action => ({ filter: action.filter, orderBy: action.orderBy })),
      exhaustMap(({ filter, orderBy }) =>
        this.carsService.getCars(filter, orderBy).pipe(
          map(cars => CarsListActions.loadNextCarsSuccess({ cars }))
        )
      )
    )
  );

  loadNextCarsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextCarsSuccess),
      map(action => action.cars),
      map(cars => CarsListActions.canLoadNextCars({ can: cars.length == ItemsTakePerLoad }))
    )
  );

  fetchFavoriteCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.fetchFavoriteCars),
      map(action => ({ filter: action.filter, orderBy: action.orderBy })),
      exhaustMap(({ filter, orderBy }) =>
        this.carsService.getUserFavoriteCars(filter, orderBy).pipe(
          map(cars => CarsListActions.fetchFavoriteCarsSuccess({ cars }))
        )
      )
    )
  );

  loadNextFavoriteCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextFavoriteCars),
      map(action => ({ filter: action.filter, orderBy: action.orderBy })),
      exhaustMap(({ filter, orderBy }) =>
        this.carsService.getUserFavoriteCars(filter, orderBy).pipe(
          map(cars => CarsListActions.loadNextFavoriteCarsSuccess({ cars }))
        )
      )
    )
  );

  loadNextFavoriteCarsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextFavoriteCarsSuccess),
      map(action => action.cars),
      map(cars => CarsListActions.canLoadNextFavoriteCars({ can: cars.length == ItemsTakePerLoad }))
    )
  );
}
