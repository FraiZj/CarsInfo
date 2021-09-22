import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountService} from "@account/services/account.service";
import {ActivatedRoute, Router} from "@angular/router";
import {map, switchMap, takeUntil} from "rxjs/operators";
import {SnackBarService} from "@core/services/snackbar.service";
import {Subject} from "rxjs";

@Component({
  selector: 'email-verification',
  template: ''
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
        const token: string | undefined = params['token'];

        if (token == null) {
          this.navigateToMainPage();
        }

        return token as string;
      }),
      switchMap(token => this.accountService.verifyEmail(token)),
      takeUntil(this.unsubscribe$)
    ).subscribe({
      next: () => {
        this.navigateToMainPage();
        this.snackBar.success('Email successfully verified.');
      },
      error: () => {
        this.snackBar.openSnackBar('Unable to verify your email. Try again.');
        this.navigateToMainPage();
      }
    });
  }

  private navigateToMainPage() {
    this.router.navigateByUrl('/cars');
  }


  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
