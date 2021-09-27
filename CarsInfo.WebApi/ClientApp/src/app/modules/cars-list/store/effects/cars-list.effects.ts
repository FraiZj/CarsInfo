import {ItemsSkipPerLoad} from '../../consts/filter-consts';
import {selectCarsFilterWithOrderBy} from '@cars-filter/store/selectors/cars-filter.selectors';
import {loginSuccess} from '@auth/store/actions/auth.actions';
import {CarsService} from '@cars/services/cars.service';
import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from '@ngrx/effects';
import * as CarsListActions from '../actions/cars-list.actions'
import {catchError, exhaustMap, map} from 'rxjs/operators';
import {ItemsTakePerLoad} from '@cars-list/consts/filter-consts';
import {Store} from '@ngrx/store';
import {handleError} from "@error-handler";

@Injectable()
export class CarsListEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    private readonly store: Store
  ) {
  }

  loginFetchCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginSuccess),
      exhaustMap(() => this.store.select(selectCarsFilterWithOrderBy).pipe(
        map(({filter, orderBy}) => CarsListActions.fetchCars({
          filter: {...filter, skip: ItemsSkipPerLoad, take: ItemsTakePerLoad},
          orderBy: orderBy
        })),
        catchError(error => handleError(error))
      ))
    )
  );

  fetchCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.fetchCars),
      map(action => ({filter: action.filter, orderBy: action.orderBy})),
      exhaustMap(({filter, orderBy}) =>
        this.carsService.getCars(filter, orderBy).pipe(
          map(cars => CarsListActions.fetchCarsSuccess({cars})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  loadNextCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextCars),
      map(action => ({filter: action.filter, orderBy: action.orderBy})),
      exhaustMap(({filter, orderBy}) =>
        this.carsService.getCars(filter, orderBy).pipe(
          map(cars => CarsListActions.loadNextCarsSuccess({cars})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  loadNextCarsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextCarsSuccess),
      map(action => action.cars),
      map(cars => CarsListActions.canLoadNextCars({can: cars.length == ItemsTakePerLoad}))
    )
  );

  fetchFavoriteCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.fetchFavoriteCars),
      map(action => ({filter: action.filter, orderBy: action.orderBy})),
      exhaustMap(({filter, orderBy}) =>
        this.carsService.getUserFavoriteCars(filter, orderBy).pipe(
          map(cars => CarsListActions.fetchFavoriteCarsSuccess({cars})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  loadNextFavoriteCars$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextFavoriteCars),
      map(action => ({filter: action.filter, orderBy: action.orderBy})),
      exhaustMap(({filter, orderBy}) =>
        this.carsService.getUserFavoriteCars(filter, orderBy).pipe(
          map(cars => CarsListActions.loadNextFavoriteCarsSuccess({cars})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  loadNextFavoriteCarsSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.loadNextFavoriteCarsSuccess),
      map(action => action.cars),
      map(cars => CarsListActions.canLoadNextFavoriteCars({can: cars.length == ItemsTakePerLoad}))
    )
  );

  fetchFavoriteCarsIds$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.fetchFavoriteCarsIds),
      exhaustMap(() => this.carsService.getCurrentUserFavoriteCarsIds().pipe(
          map(ids => CarsListActions.fetchFavoriteCarsIdsSuccess({ids})),
          catchError(error => handleError(error))
        )
      )
    )
  );

  toggleFavoriteCar$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CarsListActions.toggleFavoriteCar),
      map(action => action.id),
      exhaustMap((id) => this.carsService.toggleFavorite(id).pipe(
          map(() => CarsListActions.fetchFavoriteCarsIds()),
          catchError(error => handleError(error))
        )
      )
    )
  );
}
