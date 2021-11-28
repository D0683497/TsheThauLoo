import { IUser } from './user.models';
import { ISubstitute } from '../account/profile/manager/substitute.model';

export interface IManager extends IUser {
  managerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
  contactEmail: string;
  contactPhone: string;
  contactAddress: string;
  substitute: ISubstitute;
}
