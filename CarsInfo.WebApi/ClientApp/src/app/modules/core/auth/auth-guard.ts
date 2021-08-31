import { Observable } from 'rxjs';
import { UserClaims } from '../../auth/interfaces/user-claims';
import { CoreModule } from './../core.module';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: CoreModule })
export class AuthGuard implements CanActivate {
  constructor(
    private readonly authService: AuthService,
    private readonly dialog: MatDialog
  ) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.userClaims.pipe(map(claims => {
      if (claims && this.isInRoles(claims, route.data.roles)) {
        return true;
      }

      this.dialog.open(AuthDialogComponent, {
        data: {
          form: 'Login',
          returnUrl: state.url
        }
      });

      return false;
    }));
  }

  // map(claims => {
  //   if (claims && this.isInRoles(claims, route.data.roles)) {
  //     return true;
  //   }

  //   this.dialog.open(AuthDialogComponent, {
  //     data: {
  //       form: 'Login',
  //       returnUrl: state.url
  //     }
  //   }));

  //   return false;
  // })

  private isInRoles(currentUser: UserClaims, roles: string[]): boolean {
    return roles.some((r: string) => currentUser.roles.includes(r));
  }
}
