import { FilterWithPaginator } from './../../interfaces/filterWithPaginator';
import { OrderBy } from '@cars/enums/order-by';
import { Car } from '@cars/interfaces/car';
import { createAction, props } from '@ngrx/store';

export const fetchCars = createAction(
  '[Cars List] Fetch Cars',
  props<{ filter: FilterWithPaginator, orderBy: OrderBy }>()
);

export const fetchCarsSuccess = createAction(
  '[Cars List] Fetch Cars Success',
  props<{ cars: Car[] }>()
);

export const loadNextCars = createAction(
  '[Cars List] Load Next Cars',
  props<{ filter: FilterWithPaginator, orderBy: OrderBy }>()
);

export const loadNextCarsSuccess = createAction(
  '[Cars List] Load Next Cars Success',
  props<{ cars: Car[] }>()
);

export const fetchFavoriteCars = createAction(
  '[Cars List] Fetch Favorite Cars',
  props<{ filter: FilterWithPaginator, orderBy: OrderBy }>()
);

export const fetchFavoriteCarsSuccess = createAction(
  '[Cars List] Fetch Favorite Cars Success',
  props<{ cars: Car[] }>()
);

export const loadNextFavoriteCars = createAction(
  '[Cars List] Load Next Favorite Cars',
  props<{ filter: FilterWithPaginator, orderBy: OrderBy }>()
);

export const loadNextFavoriteCarsSuccess = createAction(
  '[Cars List] Load Next Favorite Cars Success',
  props<{ cars: Car[] }>()
);

export const canLoadNextCars = createAction(
  '[Cars List] Can Load Next Cars',
  props<{ can: boolean }>()
);

export const canLoadNextFavoriteCars = createAction(
  '[Cars List] Can Load Next Favorite Cars',
  props<{ can: boolean }>()
);

export const fetchFavoriteCarsIds = createAction(
  '[Cars List] Fetch Favorite Cars Ids'
);

export const fetchFavoriteCarsIdsSuccess = createAction(
  '[Cars List] Fetch Favorite Cars Ids Success',
  props<{ ids: number[] }>()
);

export const toggleFavoriteCar = createAction(
  '[Cars List] Toggle Favorite Car',
  props<{ id: number }>()
);
