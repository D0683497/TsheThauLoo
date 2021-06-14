import { IUserRegister } from './user-register.model';
import { ISubstituteRegister } from './substitute-register.model';

export interface IManagerRegister extends IUserRegister {
  divisionName: string;
  jobTitle: string;
  contactEmail: string;
  contactPhone: string;
  contactAddress: string;
  substitute: ISubstituteRegister;
}
