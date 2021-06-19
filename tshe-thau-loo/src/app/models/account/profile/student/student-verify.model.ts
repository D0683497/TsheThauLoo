import { IDocument } from '../../../document/document.model';

export interface IStudentVerify {
  id: string;
  studentConfirmed: boolean;
  networkId: string;
  college: string;
  department: string;
  class: string;
  description: string;
  files: IDocument[];
}
