import { IDocument } from '../../document/document.model';
import { ICompany } from '../../company/company.model';

export interface IRecruitmentCampaign {
  id: string;
  title: string;
  content: string;
  startTime: Date;
  endTime: Date;
  enableReview: boolean;
  files: IDocument[];
  company: ICompany;
}
