import { CarEditor } from '@cars/interfaces/car-editor';

export const carEditorFeatureKey = 'carEditor';

export interface CarEditorState {
  carEditor: CarEditor | null
}

export interface State {
  [carEditorFeatureKey]: CarEditorState;
}
