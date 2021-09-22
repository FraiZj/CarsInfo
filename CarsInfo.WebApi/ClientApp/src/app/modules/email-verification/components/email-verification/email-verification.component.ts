import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountService} from "@account/services/account.service";
import {ActivatedRoute, Router} from "@angular/router";
import {map, switchMap, takeUntil, tap} from "rxjs/operators";
import {SnackBarService} from "@core/services/snackbar.service";
import {Subject} from "rxjs";

@Component({
  selector: 'email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.scss']
})
export class EmailVerificationComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly accountService: AccountService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly snackBar: SnackBarService
  ) {
  }

  public ngOnInit(): void {
    this.route.queryParams.pipe(
      map(params => {
        const email: string | undefined = params['email'];

        if (email == null) {
          this.router.navigateByUrl('/cars');
        }

        return email as string;
      }),
      switchMap(email => this.accountService.verifyEmail(email)),
      tap({
        next: () => this.snackBar.success('Email successfully verified.'),
        error: () => this.snackBar.openSnackBar('Unable to verify your email. Try again.')
      }),
      takeUntil(this.unsubscribe$)
    ).subscribe(
      () => this.router.navigateByUrl('/cars')
    );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
