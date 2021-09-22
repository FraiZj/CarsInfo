import {selectAuthTokens} from '@auth/store/selectors/auth.selectors';
import {CoreModule} from '../core.module';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {Store} from '@ngrx/store';
import {first, switchMap} from 'rxjs/operators';

@Injectable({
  providedIn: CoreModule
})
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly store: Store) {
  }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.store.select(selectAuthTokens).pipe(
      first(),
      switchMap(tokens => {
        if (tokens && tokens.accessToken) {
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ` + tokens.accessToken
            }
          });
        }

        return next.handle(req);
      }))
  }
}

