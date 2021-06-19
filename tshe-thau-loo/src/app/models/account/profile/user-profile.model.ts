import { GenderType } from '../../../enums/gender-type.enum';

export interface IUserProfile {
  id: string;
  hasPhoto: boolean;
  userName: string;
  email: string;
  emailConfirmed: boolean;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  isEnable: boolean;
  identityConfirmed: boolean;
  nationalId: string;
  name: string;
  gender: GenderType;
  dateOfBirth: Date;
  currentAddress: string;
}
