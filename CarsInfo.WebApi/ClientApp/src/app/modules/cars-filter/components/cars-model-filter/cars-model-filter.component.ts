import { Component, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'cars-model-filter',
  templateUrl: './cars-model-filter.component.html',
  styleUrls: ['./cars-model-filter.component.scss']
})
export class CarsModelFilterComponent implements OnDestroy {
  @Output() public filterModelEvent = new EventEmitter<string>();
  private readonly filterDebounceTime = 400;
  private readonly subscriptions: Subscription[] = [];
  public modelFormControl = new FormControl();

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  onModelInput(): void {
    this.subscriptions.push(
      this.modelFormControl.valueChanges
        .pipe(debounceTime(this.filterDebounceTime), distinctUntilChanged())
        .subscribe(value => {
          this.filterModelEvent.emit(value);
        }));
  }
}
