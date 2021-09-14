import { Brand } from '@brands/interfaces/brand';
import { createAction, props } from "@ngrx/store";

export const fetchFilterBrands = createAction(
  '[Cars Brand Filter] Fetch Filter Brands',
  props<{ brandName?: string }>()
);

export const fetchFilterBrandsSuccess = createAction(
  '[Cars Brand Filter] Fetch Filter Brands Success',
  props<{ brands: Brand[] }>()
);
