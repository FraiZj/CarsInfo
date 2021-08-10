import { JwtInterseptor } from './helpers/jwt-interceptor';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { NavComponent } from './components/nav/nav.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatCardModule} from '@angular/material/card';
import { CarCardComponent } from './components/car-card/car-card.component';
import { RegisterComponent } from './components/register/register.component';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CarsListComponent } from './components/cars-list/cars-list.component';
import { CardDetailsComponent } from './components/card-details/card-details.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { CarEditorComponent } from './components/car-editor/car-editor.component';
import { CarsFilterComponent } from './components/cars-filter/cars-filter.component';
import {MatSelectModule} from '@angular/material/select';

const modules = [
  MatButtonModule,
  MatFormFieldModule,
  MatInputModule,
  MatIconModule,
  MatToolbarModule,
  MatCardModule,
  MatSidenavModule,
  MatListModule,
  MatSelectModule
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavComponent,
    CarCardComponent,
    RegisterComponent,
    CarsListComponent,
    CardDetailsComponent,
    NotFoundComponent,
    CarEditorComponent,
    CarsFilterComponent
  ],
  imports: [
    ...modules,
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule
  ],
  exports: [
    ...modules
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterseptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
