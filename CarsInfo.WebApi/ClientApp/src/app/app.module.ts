import { RegisterModule } from './modules/register/register.module';
import { environment } from './../environments/environment.prod';
import { CoreModule } from './modules/core/core.module';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CarsListModule } from './modules/cars-list/cars-list.module';
import { CarsListRoutingModule } from './modules/cars-list/cars-list-routing.module';
import { CommonModule } from '@angular/common';
import { CarEditorModule } from './modules/car-editor/car-editor.module';
import { JwtInterceptor } from './helpers/jwt-interceptor';

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

    // app modules
    CoreModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: "BASE_API_URL", useValue: environment.baseApiUrl }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
