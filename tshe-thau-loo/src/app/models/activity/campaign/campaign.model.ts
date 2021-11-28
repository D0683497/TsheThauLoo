import { IDocument } from '../../document/document.model';

export interface ICampaign {
  id: string;
  title: string;
  content: string;
  startTime: Date;
  endTime: Date;
  files: IDocument[];
}
