import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Roles } from '@auth/enums/roles';
import { AuthGuard } from '@core/auth/auth-guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/cars',
    pathMatch: 'full'
  },
  {
    path: '404',
    loadChildren: () => import('./modules/core/core.module').then(m => m.CoreModule)
  },

  {
    path: 'cars',
    loadChildren: () => import('./modules/cars-list/cars-list.module').then(m => m.CarsListModule)
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
