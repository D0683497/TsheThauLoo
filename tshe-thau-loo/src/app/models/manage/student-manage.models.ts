import { IUserManage } from './user-manage.models';

export interface IStudentManage extends IUserManage {
  studentConfirmed: boolean;
  networkId: string;
  college: string;
  department: string;
  class: string;
}
