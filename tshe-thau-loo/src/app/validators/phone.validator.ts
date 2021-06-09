import { AbstractControl, ValidatorFn } from '@angular/forms';
import { PhoneNumberUtil } from 'google-libphonenumber';

// https://stackoverflow.com/questions/49530023/use-google-libphonenumber-in-angular-reactive-forms-validator

// eslint-disable-next-line @typescript-eslint/naming-convention
export const PhoneNumberValidator = (regionCode: string = undefined): ValidatorFn => (control: AbstractControl): { [key: string]: any } => {
  const phoneNumberUtil = PhoneNumberUtil.getInstance();
  if (control.value === null || control.value === '') {
    return null;
  }
  try {
    const phoneNumber = phoneNumberUtil.parseAndKeepRawInput(control.value, regionCode);
    return phoneNumberUtil.isValidNumber(phoneNumber) ? null : {phone: {value: control.value}};
  } catch (e) {
    return {phone: {value: control.value}};
  }
};
