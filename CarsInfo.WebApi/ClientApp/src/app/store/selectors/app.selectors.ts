import {createFeatureSelector, createSelector, select} from "@ngrx/store";
import {appFeatureKey, AppState} from "../states/app.state";
import {pipe} from "rxjs";
import {filter, map} from "rxjs/operators";

export const selectAppState = createFeatureSelector<AppState>(appFeatureKey);

export const selectApplicationError = createSelector(
  selectAppState,
  (state) => state.applicationError
);

export const selectNotNullAppError = pipe(
  select(selectApplicationError),
  filter(error => error != null),
  map(error => error!)
);
