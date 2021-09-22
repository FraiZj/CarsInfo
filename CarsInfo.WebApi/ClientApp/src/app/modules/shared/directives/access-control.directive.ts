import { Subscription, Observable } from 'rxjs';
import * as fromAuth from 'app/modules/auth/store/selectors/auth.selectors';
import { Directive, ElementRef, Input, OnDestroy, OnInit } from "@angular/core";
import { map } from 'rxjs/operators';
import { Store } from '@ngrx/store';

@Directive({
  selector: '[accessControl]'
})
export class AccessControlDirective implements OnInit, OnDestroy {
  @Input() public roles: string[] = [];
  @Input() public authenticated: boolean = true;
  private defaultDisplay!: string;
  private subscriptions: Subscription[] = [];

  constructor(
    private elementRef: ElementRef,
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.defaultDisplay = this.elementRef.nativeElement.style.display;
    this.elementRef.nativeElement.style.display = 'none';
    this.changeElementAppearance();
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  private changeElementAppearance(): void {
    this.subscriptions.push(
      this.hasAccess()
        .subscribe(hasAccess => {
          this.elementRef.nativeElement.style.display = hasAccess ? this.defaultDisplay : 'none';
        })
    );
  }

  private hasAccess(): Observable<boolean> {
    return this.store.select(fromAuth.selectUserClaims).pipe(
      map(claims => {
        if (!this.authenticated) {
          return claims == null;
        }

        if (this.authenticated && (this.roles == null || this.roles.length == 0)) {
          return claims != null;
        }

        if (claims == null) {
          return false;
        }

        return claims.roles.some(r => this.roles.includes(r));
      })
    )
  }
}
