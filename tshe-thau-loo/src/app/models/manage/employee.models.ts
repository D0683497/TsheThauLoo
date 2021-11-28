import { IUser } from './user.models';

export interface IEmployee extends IUser {
  employeeConfirmed: boolean;
  networkId: string;
  dept: string;
  unit: string;
}
