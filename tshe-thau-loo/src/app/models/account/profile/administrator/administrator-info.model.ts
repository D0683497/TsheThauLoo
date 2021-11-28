import { IResponsibility } from './responsibility.model';

export interface IAdministratorInfo {
  id: string;
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
