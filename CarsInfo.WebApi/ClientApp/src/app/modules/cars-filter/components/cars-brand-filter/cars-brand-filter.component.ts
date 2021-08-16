import { Component, EventEmitter, HostBinding, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Brand } from 'app/modules/shared/interfaces/brand';
import { BrandsService } from 'app/modules/shared/services/brands.service';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'cars-brand-filter',
  templateUrl: './cars-brand-filter.component.html',
  styleUrls: ['./cars-brand-filter.component.scss']
})
export class CarsBrandFilterComponent implements OnInit {
  @Output() filterBrandEvent = new EventEmitter<string[]>();
  @HostBinding('style.width.px') width: number = 200;

  private static readonly WidthChangeValue: number = 100;
  private static readonly BrandsInFilterMaxValue: number = 5;
  private static readonly FilterDebounceTime: number = 400;

  public selectable = true;
  public removable = true;
  public brandFormControl: FormControl = new FormControl();
  public filteredBrands$!: Observable<Brand[]>;
  public selectedBrands: string[] = [];

  constructor(private readonly brandsService: BrandsService) { }

  public ngOnInit(): void {
    this.filteredBrands$ = this.brandsService.getBrands();
  }

  public onBrandInput(): void {
    this.brandFormControl.valueChanges
      .pipe(debounceTime(CarsBrandFilterComponent.FilterDebounceTime), distinctUntilChanged())
      .subscribe(value => {
        this.filteredBrands$ = this.brandsService.getBrands(value);
      });
  }

  public onBrandRemove(brand: string): void {
    const index = this.selectedBrands.indexOf(brand);

    if (index === -1) {
      return;
    }

    this.selectedBrands.splice(index, 1);
    this.width -= CarsBrandFilterComponent.WidthChangeValue;
    this.filterBrandEvent.emit(this.selectedBrands);
  }

  public onBrandSelect(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;

    if (this.selectedBrands.includes(value) ||
        this.selectedBrands.length >= CarsBrandFilterComponent.BrandsInFilterMaxValue) {
      return;
    }

    this.selectedBrands.push(value);
    this.width += CarsBrandFilterComponent.WidthChangeValue;
    this.brandFormControl.reset();
    this.filterBrandEvent.emit(this.selectedBrands);
  }
}
