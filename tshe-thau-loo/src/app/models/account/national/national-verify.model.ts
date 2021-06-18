import { GenderType } from '../../../enums/gender-type.enum';
import { IDocument } from '../../document/document.model';

export interface INationalVerify {
  id: string;
  identityConfirmed: boolean;
  nationalId: string;
  name: string;
  gender: GenderType;
  dateOfBirth: Date;
  currentAddress: string;
  description: string;
  files: IDocument[];
}

