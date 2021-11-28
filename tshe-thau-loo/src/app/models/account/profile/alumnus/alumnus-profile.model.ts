import { IUserProfile } from '../user-profile.model';

export interface IAlumnusProfile extends IUserProfile {
  alumnusConfirmed: boolean;
  dateOfGraduation: string;
  college: string;
  department: string;
  class: string;
}
