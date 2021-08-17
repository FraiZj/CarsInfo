import { Directive, ElementRef, HostListener, OnInit, Renderer2 } from "@angular/core";

@Directive({
  selector: '[nav-item]'
})
export class NavItemDirective implements OnInit {
  constructor(private readonly el: ElementRef, private readonly renderer: Renderer2) {}

  public ngOnInit(): void {
    this.removeUnderline();
  }

  @HostListener('mouseenter') public onMouseEnter(): void {
    this.addUnderline();
  }

  @HostListener('mouseleave') public onMouseLeave(): void {
    this.removeUnderline();
  }

  private addUnderline(): void {
    this.renderer.setStyle(this.el.nativeElement, 'border-bottom', '1px solid black');
  }

  private removeUnderline(): void {
    this.renderer.setStyle(this.el.nativeElement, 'border-bottom', '1px solid transparent');
  }
}
