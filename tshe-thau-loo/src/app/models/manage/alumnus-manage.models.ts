import { IUserManage } from './user-manage.models';

export interface IAlumnusManage extends IUserManage {
  alumnusConfirmed: boolean;
  dateOfGraduation: string;
  college: string;
  department: string;
  class: string;
}
