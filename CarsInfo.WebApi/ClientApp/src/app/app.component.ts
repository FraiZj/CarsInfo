import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from "rxjs";
import {Store} from "@ngrx/store";
import {takeUntil} from "rxjs/operators";
import {selectNotNullAppError} from "./store/selectors/app.selectors";
import {SnackBarService} from "@core/services/snackbar.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  public footerText: string = `${new Date().getUTCFullYear()} Cars Info`;
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly store: Store,
    private readonly snackBar: SnackBarService
  ) {
  }

  public ngOnInit(): void {
    this.store.pipe(selectNotNullAppError).pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(
      error => this.snackBar.openSnackBar(error)
    );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
