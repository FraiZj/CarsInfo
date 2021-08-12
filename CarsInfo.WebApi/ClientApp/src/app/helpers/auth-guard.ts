import { AuthenticationOption } from './../modules/authentication/types/authentication-option';
import { AuthenticationDialogComponent } from './../modules/authentication/components/authentication-dialog/authentication-dialog.component';
import { AuthService } from '../modules/shared/services/auth.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { User } from '../modules/shared/interfaces/user';
import { MatDialog } from '@angular/material/dialog';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private dialog: MatDialog) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const currentUser: User = this.authService.getCurrentUserValue();

    console.log(currentUser)

    if(currentUser && this.isInRoles(currentUser, route.data.roles)) {
      return true;
    }

    this.dialog.open(AuthenticationDialogComponent, {
      data: {
        form: 'Login',
        returnUrl: state.url
      }
    });

    return false;
  }

  private isInRoles(currentUser: User, roles: string[]): boolean {
    return roles.some((r : string) => currentUser.roles.includes(r));
  }
}
