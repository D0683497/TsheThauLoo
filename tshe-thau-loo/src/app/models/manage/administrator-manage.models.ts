import { IUserManage } from './user-manage.models';

export interface IAdministratorManage extends IUserManage {
  administratorConfirmed: boolean;
  showAbout: boolean;
  networkId: string;
  dept: string;
  unit: string;
  jobTitle: string;
  extension: string;
  contactEmail: string;
}
