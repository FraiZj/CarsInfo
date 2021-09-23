import {Directive, ElementRef, HostListener, Input, OnInit, Renderer2} from "@angular/core";

@Directive({
  selector: '[nav-item]'
})
export class NavItemDirective implements OnInit {
  @Input() defaultColor: string = '#cdd9e5';
  @Input() hoverColor: string = '#76828b';

  constructor(private readonly el: ElementRef, private readonly renderer: Renderer2) {
  }

  public ngOnInit(): void {
    this.mouseLeave();
  }

  @HostListener('mouseenter')
  public onMouseEnter(): void {
    this.mouseEnter();
  }

  @HostListener('mouseleave')
  public onMouseLeave(): void {
    this.mouseLeave();
  }

  private mouseEnter(): void {
    this.renderer.setStyle(this.el.nativeElement, 'color', this.hoverColor);
  }

  private mouseLeave(): void {
    this.renderer.setStyle(this.el.nativeElement, 'color', this.defaultColor);
  }
}
