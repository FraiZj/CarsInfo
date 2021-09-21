import {createReducer, on} from "@ngrx/store";
import {CoreState} from "@core/store/states/core.state";
import {addApplicationError, resetApplicationError} from "@core/store/actions/core.actions";

export const initialState: CoreState = {
  applicationError: null
};

export const reducer = createReducer(
  initialState,
  on(addApplicationError, (state, { applicationError }) => ({ ...state, applicationError })),
  on(resetApplicationError, (state) => ({ ...state, applicationError: null }))
);
