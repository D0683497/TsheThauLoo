import { IUserRegister } from './user-register.model';

export interface IEmployeeRegister extends IUserRegister {
  networkId: string;
  dept: string;
  unit: string;
}
