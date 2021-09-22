import {selectUserClaims} from '@auth/store/selectors/auth.selectors';
import {Observable} from 'rxjs';
import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot} from '@angular/router';
import {map} from 'rxjs/operators';
import {Store} from '@ngrx/store';
import {EmailVerificationModule} from "../email-verification.module";

@Injectable({providedIn: EmailVerificationModule})
export class EmailVerificationGuard implements CanActivate {
  constructor(
    private readonly store: Store
  ) {
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.store.select(selectUserClaims).pipe(
      map(claims => claims == null || !claims.emailVerified)
    );
  }
}
