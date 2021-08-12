import { Component, EventEmitter, HostBinding, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { Brand } from 'src/app/modules/shared/interfaces/brand';
import { BrandsService } from 'src/app/modules/shared/services/brands.service';

@Component({
  selector: 'cars-brand-filter',
  templateUrl: './cars-brand-filter.component.html',
  styleUrls: ['./cars-brand-filter.component.scss']
})
export class CarsBrandFilterComponent implements OnInit {
  selectable = true;
  removable = true;
  brandFormControl = new FormControl();
  filteredBrands$!: Observable<Brand[]>;
  selectedBrands: string[] = [];
  @HostBinding('style.width.px') width: number = 200;
  @Output() filterBrandEvent = new EventEmitter<string[]>();

  constructor(private brandsService: BrandsService) { }

  ngOnInit(): void {
    this.filteredBrands$ = this.brandsService.getBrands();
  }

  onBrandInput(event: Event): void {
    const value = (<HTMLInputElement>event.target).value;
    this.filteredBrands$ = this.brandsService.getBrands(value);
  }

  OnBrandRemove(brand: string): void {
    const index = this.selectedBrands.indexOf(brand);

    if (index >= 0) {
      this.selectedBrands.splice(index, 1);
      this.width -= 100;
    }

    this.filterBrandEvent.emit(this.selectedBrands);
  }

  onBrandSelect(event: MatAutocompleteSelectedEvent): void {
    const value = event.option.viewValue;
    this.filteredBrands$.subscribe(brands => {
      console.log(brands.map(b => b.name.toLowerCase()))
      const contains = brands.map(b => b.name.toLowerCase()).includes(value.toLowerCase());

      if (!contains || this.selectedBrands.length >= 5) {
        return;
      }

      this.selectedBrands.push(value);
      this.width += 100;
      this.brandFormControl.reset();
      this.filterBrandEvent.emit(this.selectedBrands);
    })
  }
}
