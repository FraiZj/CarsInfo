import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'cars-model-filter',
  templateUrl: './cars-model-filter.component.html',
  styleUrls: ['./cars-model-filter.component.scss']
})
export class CarsModelFilterComponent {
  modelFormControl = new FormControl();
  @Output() filterModelEvent = new EventEmitter<string>();

  constructor() { }

  onModelInput(event: Event): void {
    const value = (<HTMLInputElement>event.target).value;
    this.filterModelEvent.emit(value);
  }
}
