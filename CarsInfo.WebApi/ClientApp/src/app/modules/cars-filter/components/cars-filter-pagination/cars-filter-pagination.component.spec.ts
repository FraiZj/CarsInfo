import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarsFilterPaginationComponent } from './cars-filter-pagination.component';

describe('CarsFilterPaginationComponent', () => {
  let component: CarsFilterPaginationComponent;
  let fixture: ComponentFixture<CarsFilterPaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CarsFilterPaginationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CarsFilterPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
