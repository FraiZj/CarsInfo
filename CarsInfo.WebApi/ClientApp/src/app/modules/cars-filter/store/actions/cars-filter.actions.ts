import { Filters } from "@cars-filter/enums/filters";
import { createAction, props } from "@ngrx/store";
import { Filter } from "../../interfaces/filter";

export const saveFilter = createAction(
  '[Filter] Save',
  props<{ filterName: Filters, filter: Filter }>()
);

export const clearFilter = createAction(
  '[Filter] Clear',
  props<{ filterName: Filters }>()
);
