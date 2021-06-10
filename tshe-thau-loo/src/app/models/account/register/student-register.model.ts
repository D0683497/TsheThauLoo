import { IUserRegister } from './user-register.model';

export interface IStudentRegister extends IUserRegister {
  networkId: string;
  college: string;
  department: string;
  class: string;
}
