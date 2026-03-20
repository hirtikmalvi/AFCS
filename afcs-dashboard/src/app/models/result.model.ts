export interface Result<T> {
  success: boolean;
  statusCode: number;
  data: T;
  errors: string[];
}
