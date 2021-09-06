import { Filters } from './../../enums/filters';
import * as FilterActions from './../actions/cars-filter.actions';
import { createReducer, on } from '@ngrx/store';
import { FilterState } from './../states/cars-filter.state';
import { Filter } from '@cars-filter/interfaces/filter';

export const initialState: FilterState = {
  carsFilter: null,
  favoriteCarsFilter: null
};

export const reducer = createReducer(
  initialState,
  on(FilterActions.saveFilter, (state, { filterName, filter }) => handleReducer(state, filterName, filter)),
  on(FilterActions.clearFilter, (state, { filterName }) => handleReducer(state, filterName))
);

function handleReducer(state: FilterState, filterName: Filters, filter: Filter | null = null) {
  if (filterName == Filters.CarsFilter) {
    return { ...state, carsFilter: filter };
  } else {
    return { ...state, favoriteCarsFilter: filter };
  }
}
