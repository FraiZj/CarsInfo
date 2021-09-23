import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountService} from "@account/services/account.service";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {map, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {Store} from "@ngrx/store";
import {verifyEmail} from "../../store/actions/email-verification.actions";

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
    private readonly store: Store
  ) {
  }

  public ngOnInit(): void {
    const fetchToken = (params: Params): string => {
      const token: string | undefined = params['token'];

      if (token == null) {
        this.navigateToMainPage();
      }

      return token as string;
    }

    this.route.queryParams.pipe(
      map(params => fetchToken(params)),
      takeUntil(this.unsubscribe$)
    ).subscribe(
      token => this.store.dispatch(verifyEmail({ token }))
    );
  }

  private navigateToMainPage() {
    this.router.navigateByUrl('/cars');
  }


  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
