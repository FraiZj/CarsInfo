import { EffectsModule } from '@ngrx/effects';
import { environment } from './../environments/environment.prod';
import { CoreModule } from './modules/core/core.module';
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
    StoreModule.forRoot({}),
    EffectsModule.forRoot([AuthEffects]),

    // app modules
    CoreModule
  ],
  providers: [
    AsyncPipe,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: "BASE_API_URL", useValue: environment.baseApiUrl }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
