import { EffectsModule } from '@ngrx/effects';
import { environment } from '../environments/environment.prod';
import { CoreModule } from '@core/core.module';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AsyncPipe, CommonModule } from '@angular/common';
import { JwtInterceptor } from '@core/request-configuration/jwt-interceptor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { StoreModule } from '@ngrx/store';
import { AuthEffects } from '@auth/store/effects/auth.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from 'angularx-social-login';
import {EmailVerificationModule} from "./modules/email-verification/email-verification.module";
import {RefreshTokenInterceptor} from "@core/request-configuration/refresh-token-interceptor";
import {appReducer} from "./store/reducers/app.reducers";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    // library modules
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,
    NgbModule,
    StoreModule.forRoot({
      app: appReducer
    }),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      autoPause: true
    }),
    EffectsModule.forRoot([AuthEffects]),
    SocialLoginModule,

    // app modules
    CoreModule,
    EmailVerificationModule
  ],
  providers: [
    AsyncPipe,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: RefreshTokenInterceptor, multi: true },
    { provide: "BASE_API_URL", useValue: environment.baseApiUrl },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: true,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.googleClientId)
          }
        ]
      } as SocialAuthServiceConfig
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
