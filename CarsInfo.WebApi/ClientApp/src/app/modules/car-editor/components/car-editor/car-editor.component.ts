import { Observable } from 'rxjs';
import { Brand } from '../../../brands/interfaces/brand';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormArray, FormBuilder, FormGroup, Validators, AbstractControl, FormControl } from '@angular/forms';
import { BrandsService } from 'app/modules/brands/services/brands.service';
import { CarsService } from 'app/modules/cars/services/cars.service';

@Component({
  templateUrl: './car-editor.component.html',
  styleUrls: ['./car-editor.component.scss']
})
export class CarEditorComponent implements OnInit {
  public brands$!: Observable<Brand[]>;
  public brandEditorForm: FormGroup = this.formBuilder.group({
    brand: ['', [Validators.required]]
  });
  public carEditorForm: FormGroup = this.formBuilder.group({
    brandId: ['', [Validators.required]],
    model: ['', [Validators.required]],
    description: ['', [Validators.maxLength(150)]],
    carPicturesUrls: this.formBuilder.array([]),

  });
  public canAddCarPicture!: boolean;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly carsService: CarsService,
    private readonly brandsService: BrandsService,
    private readonly router: Router
  ) { }

  public get brandId(): FormControl {
    return this.carEditorForm.get('brandId') as FormControl;
  }

  public get model(): FormControl {
    return this.carEditorForm.get('model') as FormControl;
  }

  public get description(): FormControl {
    return this.carEditorForm.get('description') as FormControl;
  }

  public get carPicturesUrls(): FormArray {
    return this.carEditorForm.get('carPicturesUrls') as FormArray;
  }

  public get brand(): FormControl {
    return this.brandEditorForm.get('brand') as FormControl;
  }

  public ngOnInit(): void {
    this.brands$ = this.brandsService.getBrands();
    this.canAddCarPicture = this.carPicturesUrls.length < 3;
  }

  public addCarPicture(): void {
    if (this.carPicturesUrls.length >= 3) {
      return;
    }

    this.carPicturesUrls.push(this.formBuilder.control('', [Validators.required]));

    if (this.carPicturesUrls.length >= 3) {
      this.canAddCarPicture = false;
    }
  }

  public removeCarPicture(index: number): void {
    if (this.carPicturesUrls.at(index) != null) {
      this.carPicturesUrls.removeAt(index);
    }

    if (this.carPicturesUrls.length < 3) {
      this.canAddCarPicture = true;
    }
  }

  public addNewBrand(): void {
    this.brandsService.addBrand(this.brand?.value)
      .subscribe(() => this.brands$ = this.brandsService.getBrands());

    this.carEditorForm.controls['brand'].setValue('');
  }

  public onSubmit(): void {
    this.carsService.addCar(this.carEditorForm.value)
      .subscribe(() => this.router.navigateByUrl('/cars'));
  }
}
