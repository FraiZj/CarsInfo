import { fetchCarEditorById, updateCar } from './../../store/actions/car-editor.actions';
import { selectCarEditor } from './../../store/selectors/car-editor.selectors';
import { Store } from '@ngrx/store';
import { Observable, Subscription, throwError } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { CarEditor } from 'app/modules/cars/interfaces/car-editor';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'car-editor',
  templateUrl: './car-editor.component.html',
  styleUrls: ['./car-editor.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarEditorComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  private id!: number;
  public carEditor$: Observable<CarEditor | null> = this.store.select(selectCarEditor);

  constructor(
    private readonly carsService: CarsService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.subscriptions.push(
      this.getIdFromRoute().pipe(
        tap({
          next: id => this.id = id,
          error: () => this.router.navigateByUrl("not-found")
        })
      ).subscribe(
        id => this.store.dispatch(fetchCarEditorById({ id }))
      )
    );
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onSubmit(car: CarEditor): void {
    this.store.dispatch(updateCar({ id: this.id, carEditor: car }));
    this.subscriptions.push(
      this.carsService.updateCar(this.id, car)
        .subscribe(() => this.router.navigateByUrl(`/cars/${this.id}`))
    );
  }

  private getIdFromRoute(): Observable<number> {
    return this.route.params.pipe(map(params => {
      if (params.id == null || Number.isNaN(+params.id)) {
        throw new Error(`Invalid route param id=${params.id}`);
      }

      return params.id;
    }))
  }
}
