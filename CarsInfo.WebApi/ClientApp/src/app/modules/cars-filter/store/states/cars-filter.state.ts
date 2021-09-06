import { Filter } from './../../interfaces/filter';

export const filterFeatureKey = 'filter';

export interface FilterState {
  carsFilter: Filter | null;
  favoriteCarsFilter: Filter | null;
}

export interface State {
  [filterFeatureKey]: FilterState;
}
