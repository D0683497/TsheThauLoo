import { IUserProfile } from '../user-profile.model';
import { ISubstitute } from './substitute.model';

export interface IManagerProfile extends IUserProfile {
  managerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
  contactEmail: string;
  contactPhone: string;
  contactAddress: string;
  substitute: ISubstitute;
}
