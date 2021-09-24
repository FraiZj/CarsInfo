import {AbstractControl, ValidationErrors, ValidatorFn} from "@angular/forms";

export const checkPasswords: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
  return group.get('newPassword')!.value === group.get('confirmPassword')!.value ? null : {notSame: true}
}
