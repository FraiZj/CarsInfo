import { ClaimTypes } from './../enums/claim-types';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { JwtPayload } from "app/modules/auth/interfaces/jwt-payload";
import { UserClaims } from "app/modules/auth/interfaces/user-claims";
import { UserLogin } from "app/modules/auth/interfaces/user-login";
import { UserRegister } from "app/modules/auth/interfaces/user-register";
import jwtDecode from "jwt-decode";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, take, tap } from "rxjs/operators";
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
    this.configureCurrentUserTokens();
  }

  private configureCurrentUserTokens(): void {
    const tokensString: string | null = localStorage.getItem(AuthService.TokensName);

    if (tokensString == null) {
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(null);
      return;
    }

    const tokens: AuthTokens = JSON.parse(tokensString);
    this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(tokens);
    const jwtToken = JSON.parse(atob(this.currentUserTokenSubject.value!.accessToken.split('.')[1]));
    const expires = new Date(jwtToken.exp * 1000);

    if (Date.now() > expires.getTime()) {
      this.refreshToken().pipe(take(1)).subscribe();
      return;
    }

    this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(tokens);
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

  public getTokensFromLocalStorage(): AuthTokens | null {
    const tokensString: string | null = localStorage.getItem(AuthService.TokensName);

    if (tokensString == null) {
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(null);
      return null;
    }

    const tokens: AuthTokens = JSON.parse(tokensString);

    return tokens;
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

  public loginWithGoogle(token: string): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/login/google`, { token })
      .pipe(tap({
        next: this.authenticationSuccededHandler,
        error: this.authenticationErrorHandler
      }));
  }

  public refreshToken(): Observable<AuthTokens> {
    const tokens = this.getTokensFromLocalStorage();

    if (tokens == null) {
      return throwError(new Error('Invalid tokens'));
    }

    return this.http.post<AuthTokens>(`${this.url}/refresh-token`, tokens)
      .pipe(tap({
        next: this.authenticationSuccededHandler,
        error: () => {
          localStorage.removeItem(AuthService.TokensName);
          this.stopRefreshTokenTimer();
        }
      }));
  }

  private authenticationSuccededHandler = (tokens: AuthTokens) => {
    localStorage.setItem(AuthService.TokensName, JSON.stringify(tokens));
    this.currentUserTokenSubject.next(tokens);
    this.startRefreshTokenTimer();
  }

  private authenticationErrorHandler = (response: { applicationError: string }) => {
    return throwError(response.applicationError);
  }

  public logout(): Observable<string> {
    const logoutHandler = () => {
      localStorage.removeItem(AuthService.TokensName);
      this.currentUserTokenSubject.next(null);
      this.stopRefreshTokenTimer();
    }

    return this.http.post<string>(`${this.url}/revoke-token`, {}, { responseType: 'text' as 'json'})
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
      this.refreshToken().subscribe();
    }, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
