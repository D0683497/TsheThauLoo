import { IDocument } from '../../../document/document.model';

export interface IAlumnusVerify {
  id: string;
  alumnusConfirmed: boolean;
  dateOfGraduation: string;
  college: string;
  department: string;
  class: string;
  description: string;
  files: IDocument[];
}
