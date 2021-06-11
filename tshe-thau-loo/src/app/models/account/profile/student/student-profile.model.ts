import { IUserProfile } from '../user-profile.model';

export interface IStudentProfile extends IUserProfile {
  studentConfirmed: boolean;
  networkId: string;
  college: string;
  department: string;
  class: string;
}
