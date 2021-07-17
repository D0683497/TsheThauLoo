import { IUser } from './user.models';

export interface IStudent extends IUser {
  studentConfirmed: boolean;
  networkId: string;
  college: string;
  department: string;
  class: string;
}
