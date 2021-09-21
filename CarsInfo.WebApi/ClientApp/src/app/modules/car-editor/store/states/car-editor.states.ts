import { CarEditor } from '@cars/interfaces/car-editor';
import {ValidationError} from "@core/interfaces/error";

export const carEditorFeatureKey = 'carEditor';

export interface CarEditorState {
  carEditor: CarEditor | null,
  validationErrors: ValidationError[]
}

export interface State {
  [carEditorFeatureKey]: CarEditorState;
}
