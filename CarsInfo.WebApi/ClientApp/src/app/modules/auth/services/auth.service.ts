import { ClaimTypes } from './../enums/claim-types';
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { JwtPayload } from "app/modules/auth/interfaces/jwt-payload";
import { UserClaims } from "app/modules/auth/interfaces/user-claims";
import { UserLogin } from "app/modules/auth/interfaces/user-login";
import { UserRegister } from "app/modules/auth/interfaces/user-register";
import jwtDecode from "jwt-decode";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, tap } from "rxjs/operators";
import { AuthModule } from "../auth.module";
import { AuthTokens } from '@auth/interfaces/auth-tokens';

@Injectable({
  providedIn: AuthModule
})
export class AuthService {
  private static readonly TokensName: string = 'tokens';
  private refreshTokenTimeout!: NodeJS.Timeout;
  private currentUserTokenSubject!: BehaviorSubject<AuthTokens | null>;

  public get userClaims(): Observable<UserClaims | null> {
    return this.currentUserTokenSubject.pipe(map(value => {
      if (value == null) {
        return null;
      }

      const jwtPayload = jwtDecode<JwtPayload>(value.accessToken);
      const userClaims: UserClaims = {
        roles: this.getRoles(jwtPayload),
        id: +jwtPayload.Id,
        email: jwtPayload[ClaimTypes.Email],
        token: value.accessToken
      };

      return userClaims;
    }));
  }

  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
    const tokensString: string | null = localStorage.getItem(AuthService.TokensName);
    if (tokensString != null) {
      const tokens: AuthTokens = JSON.parse(tokensString);
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(tokens);
    } else {
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(null);
    }
  }

  public getCurrentUserClaims(): UserClaims | null {
    if (this.currentUserTokenSubject.value == null) {
      return null;
    }

    const jwtPayload = jwtDecode<JwtPayload>(this.currentUserTokenSubject.value.accessToken);
    const userClaims: UserClaims = {
      roles: this.getRoles(jwtPayload),
      id: +jwtPayload.Id,
      email: jwtPayload[ClaimTypes.Email],
      token: this.currentUserTokenSubject.value.accessToken
    };

    return userClaims;
  }

  private getRoles(jwtPayload: JwtPayload) {
    return typeof jwtPayload[ClaimTypes.Role] == 'string' ?
      [jwtPayload[ClaimTypes.Role] as string] :
      jwtPayload[ClaimTypes.Role] as string[];
  }

  public register(userRegister: UserRegister): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/register`, userRegister)
      .pipe(tap({
        next: this.authenticationSuccededHandler,
        error: this.authenticationErrorHandler
      }));
  }

  public login(userLogin: UserLogin): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/login`, userLogin)
      .pipe(tap({
        next: this.authenticationSuccededHandler,
        error: this.authenticationErrorHandler
      }));
  }
  public refreshToken(): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/refresh-token`, this.currentUserTokenSubject.value)
      .pipe(tap({
        next: this.authenticationSuccededHandler,
        error: () => {
          localStorage.removeItem(AuthService.TokensName);
          this.currentUserTokenSubject.next(null);
          this.stopRefreshTokenTimer();
        }
      }));
  }

  private authenticationSuccededHandler = (tokens: AuthTokens) => {
    localStorage.setItem(AuthService.TokensName, JSON.stringify(tokens));
    this.currentUserTokenSubject.next(tokens);
    this.startRefreshTokenTimer();
  }

  private authenticationErrorHandler = (response: HttpErrorResponse) => {
    return throwError(response.error as string);
  }

  public logout(): Observable<void> {
    const logoutHandler = () => {
      localStorage.removeItem(AuthService.TokensName);
      this.currentUserTokenSubject.next(null);
      this.stopRefreshTokenTimer();
    }

    return this.http.post<void>(`${this.url}/revoke-token`, {})
      .pipe(tap({
        next: logoutHandler,
        error: logoutHandler
      }));
  }

  public isEmailAvailable(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}/emailAvailable/${email}`);
  }

  private startRefreshTokenTimer() {
    const jwtToken = JSON.parse(atob(this.currentUserTokenSubject.value!.accessToken.split('.')[1]));
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeout = setTimeout(() => {
      alert('refresh-token')
      this.refreshToken().subscribe();
    }, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
