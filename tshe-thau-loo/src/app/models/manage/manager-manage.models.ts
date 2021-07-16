import { IUserManage } from './user-manage.models';
import { ISubstituteEdit } from '../account/profile/manager/substitute-edit.model';

export interface IManagerManage extends IUserManage {
  managerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
  contactEmail: string;
  contactPhone: string;
  contactAddress: string;
  substitute: ISubstituteEdit;
}
