import { Router } from '@angular/router';
import { AuthenticationOption } from '../../types/authentication-option';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationDialogData } from '../../interfaces/authentication-dialog-data';

@Component({
  selector: 'auth-dialog',
  templateUrl: './auth-dialog.component.html',
  styleUrls: ['./auth-dialog.component.scss']
})
export class AuthDialogComponent implements OnInit {
  private returnUrl!: string;
  public title: AuthenticationOption = 'Login'

  constructor(
    public readonly dialogRef: MatDialogRef<AuthDialogComponent>,
    private readonly router: Router,
    @Inject(MAT_DIALOG_DATA) public data: AuthenticationDialogData
  ) { }

  public ngOnInit(): void {
    this.title = this.data.form as AuthenticationOption;
    this.returnUrl = this.data.returnUrl ?? '/cars';
  }

  public switchToRegister(): void {
    this.title = 'Register';
  }

  public switchToLogin(): void {
    this.title = 'Login';
  }

  public onLogin(): void {
    this.closeDialog();
    this.router.navigateByUrl(this.returnUrl);
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }
}
