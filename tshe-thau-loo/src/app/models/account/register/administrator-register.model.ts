import { IUserRegister } from './user-register.model';

export interface IAdministratorRegister extends IUserRegister {
  networkId: string;
  dept: string;
  unit: string;
  jobTitle: string;
  extension: string;
  contactEmail: string;
}
