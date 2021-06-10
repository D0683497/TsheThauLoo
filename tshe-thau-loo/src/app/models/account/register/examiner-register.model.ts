import { IUserRegister } from './user-register.model';

export interface IExaminerRegister extends IUserRegister {
  divisionName: string;
  jobTitle: string;
}
