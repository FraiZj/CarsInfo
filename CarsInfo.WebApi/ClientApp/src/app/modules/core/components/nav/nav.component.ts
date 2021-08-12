import { AuthenticationDialogComponent } from './../../../authentication/components/authentication-dialog/authentication-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/modules/shared/services/auth.service';
import { AuthenticationOption } from 'src/app/modules/authentication/types/authentication-option';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  constructor(
    private authService: AuthService,
    private router: Router,
    public dialog: MatDialog) { }

  onLogout(): void {
    this.authService.logout();
    console.log('logout')
    this.router.navigate(['/cars']);
  }

  openDialog(form: AuthenticationOption): void {
    const dialogRef = this.dialog.open(AuthenticationDialogComponent, {
      data:{
        form
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }
}
