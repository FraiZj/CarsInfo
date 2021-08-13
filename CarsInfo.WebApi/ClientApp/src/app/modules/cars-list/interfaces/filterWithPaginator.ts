import { Filter } from './../../cars-filter/interfaces/filter';

export interface FilterWithPaginator extends Filter {
  skip: number;
  take: number;
}
