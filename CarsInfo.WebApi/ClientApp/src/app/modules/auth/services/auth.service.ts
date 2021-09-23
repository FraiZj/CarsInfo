import {HttpClient} from "@angular/common/http";
import {Inject, Injectable} from "@angular/core";
import {UserLogin} from "app/modules/auth/interfaces/user-login";
import {UserRegister} from "app/modules/auth/interfaces/user-register";
import {BehaviorSubject, Observable} from "rxjs";
import {tap} from "rxjs/operators";
import {AuthModule} from "../auth.module";
import {AuthTokens} from '@auth/interfaces/auth-tokens';

@Injectable({
  providedIn: AuthModule
})
export class AuthService {
  private static readonly TokensName: string = 'tokens';
  private currentUserTokenSubject!: BehaviorSubject<AuthTokens | null>;

  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
  }

  public getTokensFromLocalStorage(): AuthTokens | null {
    const tokensString: string | null = localStorage.getItem(AuthService.TokensName);

    if (tokensString == null) {
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(null);
      return null;
    }

    return JSON.parse(tokensString);
  }

  public removeTokensFromLocalStorage(): void {
    localStorage.removeItem(AuthService.TokensName);
  }

  public register(userRegister: UserRegister): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/register`, userRegister).pipe(
      tap(this.authenticationSucceedHandler)
    );
  }

  public login(userLogin: UserLogin): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/login`, userLogin).pipe(
      tap(this.authenticationSucceedHandler)
    );
  }

  public loginWithGoogle(token: string): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/login/google`, {token}).pipe(
      tap(this.authenticationSucceedHandler)
    );
  }

  public refreshToken(tokens: AuthTokens): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/refresh-token`, tokens).pipe(
      tap(this.authenticationSucceedHandler)
    );
  }

  private authenticationSucceedHandler = (tokens: AuthTokens) => {
    localStorage.setItem(AuthService.TokensName, JSON.stringify(tokens));
  }

  public logout(): Observable<string> {
    return this.http.post<string>(`${this.url}/revoke-token`, {}, {responseType: 'text' as 'json'})
      .pipe();
  }
}
