import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatChipsModule } from "@angular/material/chips";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { SharedModule } from "../shared/shared.module";
import { CarsBrandFilterComponent } from "./components/cars-brand-filter/cars-brand-filter.component";
import { CarsFilterComponent } from "./components/cars-filter/cars-filter.component";
import { CarsModelFilterComponent } from './components/cars-model-filter/cars-model-filter.component';
import { CarsFilterPaginationComponent } from './components/cars-filter-pagination/cars-filter-pagination.component';
import { MatSelectModule } from "@angular/material/select";
import { MatPaginatorModule } from '@angular/material/paginator'

@NgModule({
  declarations: [
    CarsFilterComponent,
    CarsBrandFilterComponent,
    CarsModelFilterComponent,
    CarsFilterPaginationComponent
  ],
  imports: [
    // library modules
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatIconModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,

    // app modules
    SharedModule
  ],
  exports: [
    CarsFilterComponent
  ]
})
export class CarsFilterModule { }
