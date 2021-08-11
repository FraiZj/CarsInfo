import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarsBrandFilterComponent } from './cars-brand-filter.component';

describe('CarsBrandFilterComponent', () => {
  let component: CarsBrandFilterComponent;
  let fixture: ComponentFixture<CarsBrandFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CarsBrandFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CarsBrandFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
