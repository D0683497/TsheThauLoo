export interface IServerError {
  type: string;
  title: string;
  status: number;
  detail: string;
  traceId?: string;
}
