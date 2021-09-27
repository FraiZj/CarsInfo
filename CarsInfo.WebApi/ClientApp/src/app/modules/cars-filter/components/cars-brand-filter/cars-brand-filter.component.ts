import { fetchFilterBrands } from './../../store/actions/cars-brand-filter.actions';
import { selectFilteredBrands } from './../../store/selectors/cars-filter.selectors';
import { Component, EventEmitter, OnInit, Output, OnDestroy, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Store } from '@ngrx/store';
import { Brand } from 'app/modules/brands/interfaces/brand';
import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'cars-brand-filter',
  templateUrl: './cars-brand-filter.component.html',
  styleUrls: ['./cars-brand-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsBrandFilterComponent implements OnInit, OnDestroy {
  @Input() public selectedBrands: string[] = [];
  @Output() public filterBrandEvent = new EventEmitter<string[]>();
  private static readonly FilterDebounceTime: number = 400;
  private readonly subscriptions: Subscription[] = [];
  public brandFormControl: FormControl = new FormControl();
  public brands$: Observable<Brand[]> = this.store.select(selectFilteredBrands);

  constructor(
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.store.dispatch(fetchFilterBrands({}));
    this.subscriptions.push(
      this.brandFormControl.valueChanges.pipe(
        debounceTime(CarsBrandFilterComponent.FilterDebounceTime),
        distinctUntilChanged()
      ).subscribe(
        brandName => this.store.dispatch(fetchFilterBrands({ brandName }))
      )
    );
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onBrandRemove(brand: string): void {
    const index = this.selectedBrands.indexOf(brand);

    if (index === -1) {
      return;
    }

    this.selectedBrands = this.selectedBrands.filter(b => b != brand);
    this.filterBrandEvent.emit(this.selectedBrands);
  }

  public onBrandSelect(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;

    if (this.selectedBrands.includes(value)) {
      this.brandFormControl.reset();
      return;
    }

    this.selectedBrands = [...this.selectedBrands, value];
    this.brandFormControl.reset();
    this.filterBrandEvent.emit(this.selectedBrands);
  }
}
