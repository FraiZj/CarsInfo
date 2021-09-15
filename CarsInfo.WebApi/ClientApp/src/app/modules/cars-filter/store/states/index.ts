import { carsSortingFeatureKey, CarsSortingState } from './cars-sorting.state';
import { brandFilterFeatureKey, BrandsFilterState } from "./cars-brand-filter.state";
import { filterFeatureKey, FilterState } from "./cars-filter.state";

export const carsFilterFeatureKey = 'carsFilter';

export interface CarsFilterState {
  [filterFeatureKey]: FilterState;
  [brandFilterFeatureKey]: BrandsFilterState;
  [carsSortingFeatureKey]: CarsSortingState;
}

export interface State {
  [carsFilterFeatureKey]: CarsFilterState;
}
