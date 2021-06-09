import { IUserRegister } from './user-register.model';

export interface IAlumnusRegister extends IUserRegister {
  dateOfGraduation: string;
  college: string;
  department: string;
  class: string;
}
