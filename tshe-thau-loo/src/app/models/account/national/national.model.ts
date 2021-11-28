import { GenderType } from '../../../enums/gender-type.enum';

export interface INational {
  id: string;
  identityConfirmed: boolean;
  nationalId: string;
  name: string;
  gender: GenderType;
  dateOfBirth: Date;
  currentAddress: string;
}
