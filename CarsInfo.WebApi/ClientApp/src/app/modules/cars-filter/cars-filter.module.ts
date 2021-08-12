
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatChipsModule } from "@angular/material/chips";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { SharedModule } from "../shared/shared.module";
import { CarsBrandFilterComponent } from "./components/cars-brand-filter/cars-brand-filter.component";
import { CarsFilterComponent } from "./components/cars-filter/cars-filter.component";

@NgModule({
  declarations: [
    CarsFilterComponent,
    CarsBrandFilterComponent
  ],
  imports: [
    // library modules
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatIconModule,
    ReactiveFormsModule,
    CommonModule,

    // app modules
    SharedModule
  ],
  exports: [
    CarsFilterComponent
  ]
})
export class CarsFilterModule { }
