import { Component, EventEmitter, Output, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'cars-model-filter',
  templateUrl: './cars-model-filter.component.html',
  styleUrls: ['./cars-model-filter.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarsModelFilterComponent {
  @Input() public model: string | undefined = '';
  @Output() public filterModelEvent = new EventEmitter<string>();

  onModelInput(): void {
    this.filterModelEvent.emit(this.model);
  }
}
