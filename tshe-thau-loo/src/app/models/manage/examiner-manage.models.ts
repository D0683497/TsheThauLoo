import { IUserManage } from './user-manage.models';

export interface IExaminerManage extends IUserManage {
  examinerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
}
