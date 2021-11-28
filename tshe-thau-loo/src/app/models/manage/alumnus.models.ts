import { IUser } from './user.models';

export interface IAlumnus extends IUser{
  alumnusConfirmed: boolean;
  dateOfGraduation: string;
  college: string;
  department: string;
  class: string;
}
