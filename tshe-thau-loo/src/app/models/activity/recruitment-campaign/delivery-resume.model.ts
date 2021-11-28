import { IDocument } from '../../document/document.model';

export interface IDeliveryResume {
  id: string;
  type: any;
  isInterview: boolean;
  isHire: boolean;
  resume: IDocument;
}
