import { Observable } from 'rxjs';
import { Brand } from '../../../shared/interfaces/brand';
import { CarsService } from '../../../shared/services/cars.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandsService } from 'src/app/modules/shared/services/brands.service';

@Component({
  templateUrl: './car-editor.component.html',
  styleUrls: ['./car-editor.component.scss']
})
export class CarEditorComponent implements OnInit {
  brands$!: Observable<Brand[]>;
  brandEditorForm: FormGroup = this.formBuilder.group({
    brand: ['', [Validators.required]]
  });
  carEditorForm: FormGroup = this.formBuilder.group({
    brandId: ['', [Validators.required]],
    model: ['', [Validators.required]],
    description:[''],
    carPicturesUrls: this.formBuilder.array([]),

  });
  canAddCarPicture!: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private carsService: CarsService,
    private brandsService: BrandsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.brands$ = this.brandsService.getBrands();
    this.canAddCarPicture = this.carPicturesUrls.length < 3;
  }

  get carPicturesUrls(): FormArray {
    return this.carEditorForm.get('carPicturesUrls') as FormArray;
  }

  get brand() {
    return this.brandEditorForm.get('brand');
  }

  addCarPicture(): void {
    if (this.carPicturesUrls.length >= 3) {
      return;
    }

    this.carPicturesUrls.push(this.formBuilder.control(''));

    if (this.carPicturesUrls.length >= 3) {
      this.canAddCarPicture = false;
    }
  }

  removeCarPicture(index: number): void {
    this.carPicturesUrls.removeAt(index);

    if (this.carPicturesUrls.length < 3) {
      this.canAddCarPicture = true;
    }
  }

  addNewBrand(): void {
    this.brandsService.addBrand(this.brand?.value)
      .subscribe(() => this.brands$ = this.brandsService.getBrands(),
      error => {
        console.log(error);
      });

    this.carEditorForm.controls['brand'].reset();
  }

  onSubmit(): void {
    this.carsService.addCar(this.carEditorForm.value)
      .subscribe(() => this.router.navigateByUrl('/cars'));
  }
}
