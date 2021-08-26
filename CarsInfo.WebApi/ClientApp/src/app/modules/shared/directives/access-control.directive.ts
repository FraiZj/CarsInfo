import { Subscription, Observable } from 'rxjs';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { Directive, ElementRef, Input, OnDestroy, OnInit } from "@angular/core";
import { map } from 'rxjs/operators';

@Directive({
  selector: '[access-control]'
})
export class AccessControlDirective implements OnInit, OnDestroy {
  @Input() public roles: string[] = [];
  @Input() public authenticated: boolean = true;
  private defaultDisplay!: string;
  private subscriptions: Subscription[] = [];

  constructor(
    private elementRef: ElementRef,
    private readonly authService: AuthService
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
    return this.authService.userClaims.pipe(map(
      claims => {
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
      }
    ));
  }
}
