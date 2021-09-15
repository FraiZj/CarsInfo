import { Filters } from '@cars-filter/enums/filters';
import { OrderBy } from '@cars/enums/order-by';
import { createAction, props } from '@ngrx/store';

export const saveOrderBy = createAction(
  '[Cars Sorting] Save',
  props<{ filterName: Filters, orderBy: OrderBy  }>()
);

export const resetOrderBy = createAction(
  '[Cars Sorting] Reset',
  props<{ filterName: Filters }>()
);
