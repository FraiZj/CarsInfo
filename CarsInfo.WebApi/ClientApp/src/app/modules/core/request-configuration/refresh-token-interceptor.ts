import {selectAuthTokens} from '@auth/store/selectors/auth.selectors';
import {CoreModule} from '../core.module';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {Store} from '@ngrx/store';
import {first, switchMap} from 'rxjs/operators';
import jwtDecode, {JwtPayload} from "jwt-decode";
import {refreshToken} from "@auth/store/actions/auth.actions";

@Injectable({
  providedIn: CoreModule
})
export class RefreshTokenInterceptor implements HttpInterceptor {
  constructor(private readonly store: Store) {
  }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.store.select(selectAuthTokens).pipe(
      first(),
      switchMap(tokens => {
        if (tokens) {
          const jwtPayload = jwtDecode<JwtPayload>(tokens.accessToken);
          const expires = new Date(jwtPayload.exp! * 1000);
          if (expires.getTime() - (60 * 1000) < Date.now()) {
            this.store.dispatch(refreshToken({tokens}));
          }
        }

        return next.handle(req);
      })
    )
  }
}

