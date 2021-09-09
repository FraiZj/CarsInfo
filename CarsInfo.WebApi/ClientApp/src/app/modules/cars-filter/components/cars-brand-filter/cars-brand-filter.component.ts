import { Component, EventEmitter, OnInit, Output, OnDestroy, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Brand } from 'app/modules/brands/interfaces/brand';
import { BrandsService } from 'app/modules/brands/services/brands.service';
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
