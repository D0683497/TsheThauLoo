import { FormGroup } from '@angular/forms';

// https://jasonwatmore.com/post/2018/11/07/angular-7-reactive-forms-validation-example

// eslint-disable-next-line @typescript-eslint/naming-convention
export const MustMatch = (controlName: string, matchingControlName: string): (formGroup: FormGroup) => void => (formGroup: FormGroup) => {
  const control = formGroup.controls[controlName];
  const matchingControl = formGroup.controls[matchingControlName];

  if (matchingControl.errors && !matchingControl.errors.mustMatch) {
    return;
  }

  if (control.value !== matchingControl.value) {
    matchingControl.setErrors({mustMatch: true});
  } else {
    matchingControl.setErrors(null);
  }
};
