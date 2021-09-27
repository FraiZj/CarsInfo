import {fetchCarById} from '../../store/actions/car-details.actions';
import {selectCar} from '../../store/selectors/car-details.selectors';
import {Store} from '@ngrx/store';
import {Observable, Subject} from 'rxjs';
import {ChangeDetectionStrategy, Component, OnInit, OnDestroy, ChangeDetectorRef} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Car} from 'app/modules/cars/interfaces/car';
import {map, takeUntil, tap} from 'rxjs/operators';
import {MatDialog} from '@angular/material/dialog';
import {CarDeleteConfirmDialogComponent} from "../../../car-delete-confirm-dialog/components/car-delete-confirm-dialog/car-delete-confirm-dialog.component";

@Component({
  selector: 'card-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarDetailsComponent implements OnInit, OnDestroy {
  public car$: Observable<Car | null> = this.store.select(selectCar).pipe(
    tap({
      error: () => this.router.navigateByUrl("not-found")
    })
  );
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly dialog: MatDialog,
    private readonly store: Store,
    private readonly cdr: ChangeDetectorRef
  ) {
  }

  public ngOnInit(): void {
    this.cdr.detectChanges();
    this.getIdFromRoute().pipe(
      tap({
        error: () => this.router.navigateByUrl("not-found")
      }),
      takeUntil(this.unsubscribe$)
    ).subscribe(
      id => this.store.dispatch(fetchCarById({id: id as number}))
    );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onDelete(): void {
    this.dialog.open(CarDeleteConfirmDialogComponent, {
      data: {
        car$: this.car$
      }
    });
  }

  private getIdFromRoute(): Observable<number | null> {
    return this.route.params.pipe(
      map(params => {
        if (params.id == null || Number.isNaN(+params.id)) {
          throw new Error(`Invalid route param id=${params.id}`);
        }

        return params.id as number;
      })
    );
  }
}
