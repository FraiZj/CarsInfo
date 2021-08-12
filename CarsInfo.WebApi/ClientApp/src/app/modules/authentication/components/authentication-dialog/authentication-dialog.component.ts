import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationOption } from './../../types/authentication-option';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationDialogData } from '../../interfaces/authentication-dialog-data';

@Component({
  selector: 'app-authentication-dialog',
  templateUrl: './authentication-dialog.component.html',
  styleUrls: ['./authentication-dialog.component.scss']
})
export class AuthenticationDialogComponent implements OnInit {
  title: AuthenticationOption = 'Login'
  private returnUrl!: string;

  constructor(
    public dialogRef: MatDialogRef<AuthenticationDialogComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: AuthenticationDialogData
    ) { }

  ngOnInit(): void {
    this.title = this.data.form as AuthenticationOption;
    this.returnUrl = this.data.returnUrl ?? '/cars';
  }

  switchToRegister() {
    this.title = 'Register';
  }

  switchToLogin() {
    this.title = 'Login';
  }

  onLogin() {
    this.closeDialog();
    this.router.navigateByUrl(this.returnUrl);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
