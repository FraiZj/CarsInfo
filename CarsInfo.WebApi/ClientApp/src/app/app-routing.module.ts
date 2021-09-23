import {NotFoundComponent} from '@core/components/not-found/not-found.component';
import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {Roles} from '@auth/enums/roles';
import {AuthGuard} from '@core/auth/auth-guard';
import {EmailVerificationGuard} from "./modules/email-verification/guards/email-verification-guard";

const routes: Routes = [
  {
    path: '',
    redirectTo: '/cars',
    pathMatch: 'full'
  },
  {
    path: 'cars',
    loadChildren: () => import('./modules/cars-list/cars-list.module').then(m => m.CarsListModule)
  },
  {
    path: 'filter',
    loadChildren: () => import('./modules/cars-filter/cars-filter.module').then(m => m.CarsFilterModule)
  },
  {
    path: 'cars/:id/edit',
    loadChildren: () => import('./modules/car-editor/car-editor.module').then(m => m.CarEditorModule),
    canActivate: [AuthGuard],
    data: {
      roles: [Roles.Admin]
    }
  },
  {
    path: 'cars/:id',
    loadChildren: () => import('./modules/car-details/car-details.module').then(m => m.CarDetailsModule)
  },
  {
    path: 'add-car',
    loadChildren: () => import('./modules/car-creation/car-creation.module').then(m => m.CarCreationModule),
    canActivate: [AuthGuard],
    data: {
      roles: [Roles.Admin]
    }
  },
  {
    path: 'email/verify',
    loadChildren: () => import('./modules/email-verification/email-verification.module')
      .then(m => m.EmailVerificationModule),
    canActivate: [EmailVerificationGuard]
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
