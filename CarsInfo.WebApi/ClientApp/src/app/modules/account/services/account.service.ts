import {Inject, Injectable} from '@angular/core';
import {AccountModule} from "../account.module";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResetPasswordPayload} from "@account/interfaces/reset-password-payload";

@Injectable({
  providedIn: AccountModule
})
export class AccountService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient
  ) {
  }

  public verifyEmail(token: string): Observable<void> {
    return this.http.post<void>(`${this.url}/verify-email?token=${token}`, {});
  }

  public sendVerificationEmail(): Observable<void> {
    return this.http.post<void>(`${this.url}/send-verification-email`, {});
  }

  public sendResetPasswordEmail(email: string): Observable<void> {
    return this.http.post<void>(`${this.url}/send-reset-password-email?email=${email}`, {});
  }

  public resetPassword(payload: ResetPasswordPayload): Observable<void> {
    return this.http.post<void>(`${this.url}/reset-password?token=${payload.token}`, payload);
  }
}
