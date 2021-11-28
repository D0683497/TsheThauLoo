import { GenderType } from '../../../enums/gender-type.enum';

export interface INationalEdit {
  nationalId: string;
  name: string;
  gender: GenderType;
  dateOfBirth: Date;
  currentAddress: string;
}
