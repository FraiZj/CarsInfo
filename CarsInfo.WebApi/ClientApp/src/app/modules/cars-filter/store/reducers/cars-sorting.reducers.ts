import { saveOrderBy, resetOrderBy } from './../actions/cars-sorting.actions';
import { OrderBy } from '@cars/enums/order-by';
import { CarsSortingState } from './../states/cars-sorting.state';
import { createReducer, on } from '@ngrx/store';
import { Filters } from '@cars-filter/enums/filters';

export const initialState: CarsSortingState = {
  carsListOrderBy: OrderBy.BrandNameAsc,
  favoriteCarsListOrderBy: OrderBy.BrandNameAsc
};

export const reducer = createReducer(
  initialState,
  on(saveOrderBy, (state, { filterName, orderBy }) => handleReducer(state, filterName, orderBy)),
  on(resetOrderBy, (state, { filterName }) =>handleReducer(state, filterName))
);

function handleReducer(state: CarsSortingState, filterName: Filters, orderBy: OrderBy = OrderBy.BrandNameAsc) {
  if (filterName == Filters.CarsFilter) {
    return { ...state, carsListOrderBy: orderBy };
  } else {
    return { ...state, favoriteCarsListOrderBy: orderBy };
  }
}
