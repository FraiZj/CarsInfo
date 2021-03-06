import {selectBrands} from './../../store/selectors/car-editor-form.selectors';
import {fetchBrands, createBrand} from './../../store/actions/car-editor-form.actions';
import {Store} from '@ngrx/store';
import {Observable} from 'rxjs';
import {Brand} from '@brands/interfaces/brand';
import {
  Component,
  Input,
  OnInit,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
  OnChanges, SimpleChanges
} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators, FormControl} from '@angular/forms';
import {CarEditor} from 'app/modules/cars/interfaces/car-editor';
import {ValidationError} from "@core/interfaces/error";
import {SnackBarService} from "@core/services/snackbar.service";

@Component({
  selector: 'car-editor-form',
  templateUrl: './car-editor-form.component.html',
  styleUrls: ['./car-editor-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarEditorFormComponent implements OnInit, OnChanges {
  @Input() public carEditor: CarEditor | null = null;
  @Input() public validationErrors!: ValidationError[] | null;
  @Output() public carEditorSubmit: EventEmitter<CarEditor> = new EventEmitter<CarEditor>();
  public brands$: Observable<Brand[]> = this.store.select(selectBrands);
  public brandEditorForm: FormGroup = this.formBuilder.group({
    brand: ['', [Validators.required]]
  });
  public carEditorForm: FormGroup = this.formBuilder.group({
    brandId: ['', [Validators.required]],
    model: ['', [Validators.required]],
    description: ['', [Validators.maxLength(150)]],
    carPicturesUrls: this.formBuilder.array([
      ['', Validators.required]
    ], Validators.required),

  });
  public canAddCarPicture!: boolean;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly store: Store,
    private readonly snackBar: SnackBarService
  ) {
  }

  public ngOnInit(): void {
    this.store.dispatch(fetchBrands());
    this.canAddCarPicture = this.carPicturesUrls.length < 3;
    this.configureValidationErrors();

    if (this.carEditor != null) {
      this.carEditorForm.patchValue(this.carEditor);
      this.carEditor.carPicturesUrls.forEach(pictureUrl => {
        this.carPicturesUrls.push(this.formBuilder.control(pictureUrl, [Validators.required]));
      });
    }

    this.canAddCarPicture = this.carPicturesUrls.length < 3;
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.configureValidationErrors();
  }

  private configureValidationErrors() {
    if (this.validationErrors != null) {
      this.validationErrors.forEach(({field, error}) => {
        let formControl = this.carEditorForm.get(field);

        if (field.includes('[')) {
          const carPictureIndex: string = field.substring(field.indexOf('[') + 1, field.indexOf(']'));
          formControl = this.carPicturesUrls.get(carPictureIndex);

        }

        if (formControl) {
          formControl.setErrors({
            serverError: error
          });
        }
      });
    }
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

    this.store.dispatch(createBrand({brand: this.brand.value}));
    this.brandEditorForm.controls['brand'].setValue('');
  }

  public onSubmit(): void {
    if (this.carEditorForm.invalid) {
      this.snackBar.openSnackBar('Form data is invalid.');
      return;
    }

    this.carEditorSubmit.emit(this.carEditorForm.value);
  }

  public returnBack(): void {
    window.history.back();
  }
}
