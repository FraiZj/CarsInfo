import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./modules/core/core.module').then(m => m.CoreModule)
  },
  {
    path: 'cars',
    loadChildren: () => import('./modules/cars-list/cars-list.module').then(m => m.CarsListModule)
  },
  {
    path: 'new-car',
    loadChildren: () => import('./modules/car-editor/car-editor.module').then(m => m.CarEditorModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
