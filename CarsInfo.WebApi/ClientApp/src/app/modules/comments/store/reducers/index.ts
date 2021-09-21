import {Action, combineReducers} from "@ngrx/store";
import {commentFormReducer} from "./comment-form.reducers";
import {commentFormFeatureKey} from "../states/comment-form.state";
import {commentsListReducer} from "./comments.reducers";
import {commentsListFeatureKey} from "../states/comments.state";
import {CommentsState} from "../states";

export function reducers(state: CommentsState | undefined, action: Action) {
  return combineReducers({
    [commentsListFeatureKey]: commentsListReducer,
    [commentFormFeatureKey]: commentFormReducer
  })(state, action);
}
