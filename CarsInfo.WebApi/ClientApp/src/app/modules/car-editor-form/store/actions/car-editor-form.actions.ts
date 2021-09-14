import { Brand } from "@brands/interfaces/brand";
import { createAction, props } from "@ngrx/store";

export const fetchBrands = createAction(
  '[Car Editor Form] Fetch Brands',
);

export const fetchBrandsSuccess = createAction(
  '[Car Editor Form] Fetch Brands Succcess',
  props<{ brands: Brand[] }>()
);

export const createBrand = createAction(
  '[Car Editor Form] Create Brand',
  props<{ brand: string }>()
);
