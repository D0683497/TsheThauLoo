import { ISubstitute } from './substitute.model';

export interface IManagerInfo {
  id: string;
  managerConfirmed: boolean;
  divisionName: string;
  jobTitle: string;
  contactEmail: string;
  contactPhone: string;
  contactAddress: string;
  substitute: ISubstitute;
}
