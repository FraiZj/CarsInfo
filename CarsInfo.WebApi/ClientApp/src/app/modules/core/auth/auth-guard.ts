import { selectUserClaims } from '@auth/store/selectors/auth.selectors';
import * as fromAuth from './../../auth/store/actions/auth.actions';
import { Observable } from 'rxjs';
import { UserClaims } from '@auth/interfaces/user-claims';
import { CoreModule } from './../core.module';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs/operators';
import { Store } from '@ngrx/store';

@Injectable({ providedIn: CoreModule })
export class AuthGuard implements CanActivate {
  constructor(
    private readonly store: Store
  ) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.store.select(selectUserClaims).pipe(map(claims => {
      if (claims && this.isInRoles(claims, route.data.roles)) {
        return true;
      }

      this.store.dispatch(fromAuth.loginRedirect({ returnUrl: state.url }));

      return false;
    }));
  }

  private isInRoles(currentUser: UserClaims, roles: string[]): boolean {
    return roles.some((r: string) => currentUser.roles.includes(r));
  }
}
