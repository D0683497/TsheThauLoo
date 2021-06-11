import { IUserProfile } from '../user-profile.model';
import { IResponsibility } from './responsibility.model';

export interface IAdministratorProfile extends IUserProfile {
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
