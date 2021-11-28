import { IUser } from './user.models';

export interface IExaminer extends IUser {
  examinerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
}
