import { IUserProfile } from '../user-profile.model';

export interface IEmployeeProfile extends IUserProfile {
  employeeConfirmed: boolean;
  networkId: string;
  dept: string;
  unit: string;
}
