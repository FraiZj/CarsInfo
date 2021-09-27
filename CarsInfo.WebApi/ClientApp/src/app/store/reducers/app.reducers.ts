import {AppState} from "../states/app.state";
import {createReducer, on} from "@ngrx/store";
import {addApplicationError, resetApplicationError} from "../actions/app.actions";

export const initialState: AppState = {
  applicationError: null
};

export const appReducer = createReducer(
  initialState,
  on(addApplicationError, (state, {applicationError}) => ({...state, applicationError })),
  on(resetApplicationError, (state) => ({...state, applicationError: null }))
)
