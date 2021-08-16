import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'cars-model-filter',
  templateUrl: './cars-model-filter.component.html',
  styleUrls: ['./cars-model-filter.component.scss']
})
export class CarsModelFilterComponent {
  private readonly filterDebounceTime = 400;
  modelFormControl = new FormControl();
  @Output() filterModelEvent = new EventEmitter<string>();

  onModelInput(event: Event): void {
    this.modelFormControl.valueChanges
      .pipe(debounceTime(this.filterDebounceTime), distinctUntilChanged())
      .subscribe(value => {
        this.filterModelEvent.emit(value);
      });
  }
}
