import {Component, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {checkPasswords} from "./valiadators/confirm-passwords-validator";
import {resetPassword} from "../../store/actions/reset-password.actions";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {Subject} from "rxjs";
import {map, takeUntil} from "rxjs/operators";

@Component({
  selector: 'reset-password-page',
  templateUrl: './reset-password-page.component.html',
  styleUrls: ['./reset-password-page.component.scss']
})
export class ResetPasswordPageComponent implements OnInit {
  private unsubscribe$: Subject<void> = new Subject<void>();
  private token!: string;
  public readonly resetPasswordFormGroup = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
  }, {
    validators: checkPasswords
  });

  constructor(
    private readonly store: Store,
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {
  }

  public ngOnInit(): void {
    const fetchToken = (params: Params): string => {
      const token: string | undefined = params['token'];

      if (token == null) {
        this.router.navigateByUrl('/cars');
      }

      return token as string;
    }

    this.route.queryParams.pipe(
      map(params => fetchToken(params)),
      takeUntil(this.unsubscribe$)
    ).subscribe(
      token => this.token = token
    );
  }

  public get newPassword(): FormControl {
    return this.resetPasswordFormGroup.get('newPassword') as FormControl;
  }

  public get confirmPassword(): FormControl {
    return this.resetPasswordFormGroup.get('confirmPassword') as FormControl;
  }

  public onSubmit(): void {
    if (this.resetPasswordFormGroup.invalid) {
      return;
    }

    this.store.dispatch(resetPassword({
      payload: {
        token: this.token,
        password: this.newPassword.value
      }
    }));
    this.resetPasswordFormGroup.reset();
  }
}
