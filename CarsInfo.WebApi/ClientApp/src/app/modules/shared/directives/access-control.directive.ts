import { UserClaims } from 'app/modules/auth/interfaces/user-claims';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { Directive, ElementRef, Input, OnInit } from "@angular/core";

@Directive({
  selector: '[access-control]'
})
export class AccessControlDirective implements OnInit {
  @Input() public roles: string[] = [];
  @Input() public authenticated: boolean = true;
  private defaultDisplay!: string;

  constructor(
    private elementRef: ElementRef,
    private readonly authService: AuthService
  ) { }

  public ngOnInit(): void {
    this.defaultDisplay = this.elementRef.nativeElement.style.display;
    this.elementRef.nativeElement.style.display = 'none';
    this.changeElementAppearance();
  }

  private changeElementAppearance(): void {
    this.elementRef.nativeElement.style.display = this.hasAccess() ? this.defaultDisplay : 'none';
  }

  private hasAccess(): boolean {
    const userClaims: UserClaims | null = this.authService.getCurrentUserClaims();

    if (!this.authenticated) {
      return userClaims == null;
    }

    if (this.authenticated && (this.roles == null || this.roles.length == 0)) {
      return userClaims != null;
    }

    return userClaims?.roles.some(r => this.roles.includes(r)) ?? false;
  }
}
