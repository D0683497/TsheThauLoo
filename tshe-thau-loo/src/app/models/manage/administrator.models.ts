import { IResponsibility } from '../account/profile/administrator/responsibility.model';
import { IUser } from './user.models';

export interface IAdministrator extends IUser {
  administratorConfirmed: boolean;
  showAbout: boolean;
  networkId: string;
  dept: string;
  unit: string;
  jobTitle: string;
  extension: string;
  contactEmail: string;
  responsibilities: IResponsibility[];
}
