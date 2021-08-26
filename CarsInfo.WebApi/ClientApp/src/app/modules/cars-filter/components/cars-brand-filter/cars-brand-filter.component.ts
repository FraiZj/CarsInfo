import { Component, EventEmitter, HostBinding, OnInit, Output, OnDestroy, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Brand } from 'app/modules/brands/interfaces/brand';
import { BrandsService } from 'app/modules/brands/services/brands.service';
import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'cars-brand-filter',
  templateUrl: './cars-brand-filter.component.html',
  styleUrls: ['./cars-brand-filter.component.scss']
})
export class CarsBrandFilterComponent implements OnInit, OnDestroy {
  @Input() public selectedBrands: string[] = [];
  @Output() public filterBrandEvent = new EventEmitter<string[]>();
  private static readonly BrandsInFilterMaxValue: number = 5;
  private static readonly FilterDebounceTime: number = 400;
  private readonly subscriptions: Subscription[] = [];
  public selectable = true;
  public removable = true;
  public brandFormControl: FormControl = new FormControl();
  public filteredBrands$!: Observable<Brand[]>;


  constructor(private readonly brandsService: BrandsService) { }

  public ngOnInit(): void {
    this.filteredBrands$ = this.brandsService.getBrands();
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onBrandInput(): void {
    this.subscriptions.push(this.brandFormControl.valueChanges
      .pipe(debounceTime(CarsBrandFilterComponent.FilterDebounceTime), distinctUntilChanged())
      .subscribe(value => {
        this.filteredBrands$ = this.brandsService.getBrands(value);
      }));
  }

  public onBrandRemove(brand: string): void {
    const index = this.selectedBrands.indexOf(brand);

    if (index === -1) {
      return;
    }

    this.selectedBrands.splice(index, 1);
    this.filterBrandEvent.emit(this.selectedBrands);
  }

  public onBrandSelect(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;

    if (this.selectedBrands.includes(value) ||
      this.selectedBrands.length >= CarsBrandFilterComponent.BrandsInFilterMaxValue) {
      this.brandFormControl.reset();
      return;
    }

    this.selectedBrands.push(value);
    this.brandFormControl.reset();
    this.filterBrandEvent.emit(this.selectedBrands);
  }
}
