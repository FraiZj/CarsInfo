import {ValidationError} from "@core/interfaces/error";

export interface ErrorResponse {
  applicationError: string | null;
  validationErrors: ValidationError[];
}
