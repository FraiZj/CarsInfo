import {Component} from '@angular/core';
import {Store} from "@ngrx/store";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {sendResetPassword} from "../../store/actions/reset-password.actions";

@Component({
  selector: 'send-reset-password',
  templateUrl: './send-reset-password.component.html',
  styleUrls: ['./send-reset-password.component.scss']
})
export class SendResetPasswordComponent {
  public readonly resetPasswordFormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  constructor(
    private readonly store: Store,
    private readonly fb: FormBuilder
  ) {
  }
  public get email(): FormControl {
    return this.resetPasswordFormGroup.get('email') as FormControl;
  }

  public onSubmit(): void {
    if (this.resetPasswordFormGroup.invalid) {
      return;
    }

    this.store.dispatch(sendResetPassword({
      email: this.email.value
    }));
    this.resetPasswordFormGroup.reset();
  }
}
