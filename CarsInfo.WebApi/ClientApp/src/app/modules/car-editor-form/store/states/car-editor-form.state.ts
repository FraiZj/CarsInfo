import { Brand } from "@brands/interfaces/brand";

export const carEditorFormFeatureKey = 'carEditorForm';

export interface CarEditorFormState {
  brands: Brand[]
}

export interface State {
  [carEditorFormFeatureKey]: CarEditorFormState;
}
