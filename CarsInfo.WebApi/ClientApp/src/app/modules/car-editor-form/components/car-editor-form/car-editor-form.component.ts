import { Observable, Subscription } from 'rxjs';
import { Brand } from '../../../brands/interfaces/brand';
import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { BrandsService } from 'app/modules/brands/services/brands.service';
import { CarEditor } from 'app/modules/cars/interfaces/car-editor';

@Component({
  selector: 'car-editor-form',
  templateUrl: './car-editor-form.component.html',
  styleUrls: ['./car-editor-form.component.scss']
})
export class CarEditorFormComponent implements OnInit, OnDestroy {
  @Input() public carEditor: CarEditor | null = null;
  @Output() public carEditorSubmit: EventEmitter<CarEditor> = new EventEmitter<CarEditor>();
  private readonly subscriptions: Subscription[] = [];
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
    private readonly brandsService: BrandsService,
  ) { }

  public ngOnInit(): void {
    this.brands$ = this.brandsService.getBrands();
    this.canAddCarPicture = this.carPicturesUrls.length < 3;

    if (this.carEditor != null) {
      this.carEditorForm.patchValue(this.carEditor);
      this.carEditor.carPicturesUrls.forEach(pictureUrl => {
        this.carPicturesUrls.push(this.formBuilder.control(pictureUrl, [Validators.required]));
      });
    }

    this.canAddCarPicture = this.carPicturesUrls.length < 3;
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

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
    if (this.brand?.value == null) {
      return;
    }

    this.subscriptions.push(
      this.brandsService.addBrand(this.brand.value)
        .subscribe(() => this.brands$ = this.brandsService.getBrands())
    );

    this.carEditorForm.controls['brand'].setValue('');
  }

  public onSubmit(): void {
    if (this.carEditorForm.invalid) {
      alert('Form data is invalid.');
      return;
    }

    this.carEditorSubmit.emit(this.carEditorForm.value);
  }
}
