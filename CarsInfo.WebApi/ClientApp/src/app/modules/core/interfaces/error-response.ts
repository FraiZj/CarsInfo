export interface ErrorResponse {
  applicationError: string | null;
  validationErrors: Error[];
}