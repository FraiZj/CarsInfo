import { UserClaims } from './../../shared/interfaces/user-claims';
import { CoreModule } from './../core.module';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AuthenticationDialogComponent } from '../../authentication/components/authentication-dialog/authentication-dialog.component';
import { AuthService } from 'app/modules/auth/services/auth.service';

@Injectable({providedIn: CoreModule})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private dialog: MatDialog
    ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const currentUser: UserClaims | null = this.authService.getCurrentUserClaims();

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

  private isInRoles(currentUser: UserClaims, roles: string[]): boolean {
    return roles.some((r : string) => currentUser.roles.includes(r));
  }
}
