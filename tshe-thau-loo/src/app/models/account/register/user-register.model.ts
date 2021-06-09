import { GenderType } from '../../../enums/gender-type.enum';

export interface IUserRegister {
  userName: string;
  password: string;
  passwordConfirm: string;
  email: string;
  phoneNumber: string;
  nationalId: string;
  name: string;
  gender: GenderType;
  dateOfBirth: Date;
  currentAddress: string;
}
