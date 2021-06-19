import { IResponsibility } from './account/profile/administrator/responsibility.model';

export interface IAbout {
  name: string;
  jobTitle: string;
  extension: string;
  contactEmail: string;
  responsibilities: IResponsibility[];
}
