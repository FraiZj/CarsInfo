import { OrderBy } from '@cars/enums/order-by';

export const carsSortingFeatureKey = 'carsSorting';

export interface CarsSortingState {
  carsListOrderBy: OrderBy;
  favoriteCarsListOrderBy: OrderBy;
}
