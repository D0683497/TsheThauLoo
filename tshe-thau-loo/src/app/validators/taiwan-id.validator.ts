import { AbstractControl, ValidatorFn } from '@angular/forms';
import { isNationalIdentificationNumberValid } from 'taiwan-id-validator2';

// eslint-disable-next-line @typescript-eslint/naming-convention
export const NationalIdValidator = (): ValidatorFn => (control: AbstractControl): { [key: string]: any } => {
  if (control.value === null || control.value === '') {
    return null;
  }
  if (isNationalIdentificationNumberValid(control.value)) {
    return null;
  }
  return {nationalId: {value: control.value}};
};
