import { AbstractControl, ValidatorFn } from '@angular/forms';
import { isGuiNumberValid } from 'taiwan-id-validator2';

// eslint-disable-next-line @typescript-eslint/naming-convention
export const RegistrationNumberValidator = (): ValidatorFn => (control: AbstractControl): { [key: string]: any } => {
  if (control.value === null || control.value === '') {
    return null;
  }
  if (isGuiNumberValid(control.value)) {
    return null;
  }
  return {registrationNumber: {value: control.value}};
};
