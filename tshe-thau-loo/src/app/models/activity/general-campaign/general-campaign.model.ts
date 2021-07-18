import { IDocument } from '../../document/document.model';
import { ICompany } from '../../company/company.model';

export interface IGeneralCampaign {
  id: string;
  title: string;
  content: string;
  declaration: string;
  venue: string;
  registrationStartTime: Date;
  registrationEndTime: Date;
  startTime: Date;
  endTime: Date;
  limitNumberOfPeople: number;
  enableVerify: boolean;
  enableIdentityConfirmed: boolean;
  files: IDocument[];
  company: ICompany;
}
