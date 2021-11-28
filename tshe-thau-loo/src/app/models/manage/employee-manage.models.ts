import { IUserManage } from './user-manage.models';

export interface IEmployeeManage extends IUserManage {
  employeeConfirmed: boolean;
  networkId: string;
  dept: string;
  unit: string;
}
