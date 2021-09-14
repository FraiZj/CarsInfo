import { fetchFilterBrandsSuccess } from './../actions/cars-brand-filter.actions';
import { createReducer, on } from '@ngrx/store';
import { BrandsFilterState } from './../states/cars-brand-filter.state';

export const initialState: BrandsFilterState = {
  brands: []
};

export const reducer = createReducer(
  initialState,
  on(fetchFilterBrandsSuccess, (state, { brands }) => ({ ...state, brands }))
);
