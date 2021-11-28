import { IUserProfile } from '../user-profile.model';

export interface IExaminerProfile extends IUserProfile {
  examinerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
}
