import { UsersFilter } from '@users-shared';
export const usersFilterFeatureKey = 'usersFilter';

export interface UsersFilterState {
  filter: UsersFilter | null
}

export interface State {
  [usersFilterFeatureKey]: UsersFilterState;
}