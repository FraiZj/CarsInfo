import { AuthGuard } from './helpers/auth-guard';
import { CardDetailsComponent } from './components/card-details/card-details.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarsListComponent } from './components/cars-list/cars-list.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { Roles } from './helpers/roles';
import { CarEditorComponent } from './components/car-editor/car-editor.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'cars',
    component: CarsListComponent
  },
  {
    path: 'cars/:id',
    component: CardDetailsComponent
  },
  {
    path: '404',
    component: NotFoundComponent
  },
  {
    path: 'addCar',
    component: CarEditorComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [Roles.Admin]
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
