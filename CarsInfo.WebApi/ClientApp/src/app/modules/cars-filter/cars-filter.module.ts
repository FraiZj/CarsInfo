import { CarsModule } from './../cars/cars.module';
import { BrandsModule } from './../brands/brands.module';
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatChipsModule } from "@angular/material/chips";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { SharedModule } from "../shared/shared.module";
import { CarsBrandFilterComponent } from "./components/cars-brand-filter/cars-brand-filter.component";
import { CarsFilterComponent } from "./components/cars-filter/cars-filter.component";
import { CarsModelFilterComponent } from './components/cars-model-filter/cars-model-filter.component';
import { MatSelectModule } from "@angular/material/select";
import { MatPaginatorModule } from '@angular/material/paginator'
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CarFilterMobileComponent } from './components/car-filter-mobile/car-filter-mobile.component';
import { CarsFilterRoutingModule } from './cars-filter-routing.module';
import { CarsSortingSelectComponent } from './components/cars-sorting-select/cars-sorting-select.component';

@NgModule({
  declarations: [
    CarsFilterComponent,
    CarsBrandFilterComponent,
    CarsModelFilterComponent,
    CarFilterMobileComponent,
    CarsSortingSelectComponent
  ],
  imports: [
    // library modules
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,

    // app modules
    CarsModule,
    SharedModule,
    BrandsModule,
    CarsFilterRoutingModule
  ],
  exports: [
    CarsFilterComponent,
    CarsFilterRoutingModule,
    CarFilterMobileComponent,
    CarsSortingSelectComponent
  ]
})
export class CarsFilterModule { }
