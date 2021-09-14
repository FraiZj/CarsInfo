import { brandFilterFeatureKey, BrandsFilterState } from "./cars-brand-filter.state";
import { filterFeatureKey, FilterState } from "./cars-filter.state";

export const carsFilterFeatureKey = 'carsFilter';

export interface CarsFilterState {
  [filterFeatureKey]: FilterState;
  [brandFilterFeatureKey]: BrandsFilterState
}

export interface State {
  [carsFilterFeatureKey]: CarsFilterState;
}
