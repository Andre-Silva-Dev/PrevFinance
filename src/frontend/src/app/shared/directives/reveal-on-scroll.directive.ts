import { Directive, ElementRef, Input, OnDestroy, OnInit, Renderer2, inject } from '@angular/core';

@Directive({
  selector: '[appRevealOnScroll]'
})
export class RevealOnScrollDirective implements OnInit, OnDestroy {
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly renderer = inject(Renderer2);
  private observer?: IntersectionObserver;

  @Input() revealDelay = 0;
  @Input() revealOnce = true;

  public ngOnInit(): void {
    const element = this.elementRef.nativeElement;

    this.renderer.addClass(element, 'reveal');
    this.renderer.setStyle(element, '--reveal-delay', `${this.revealDelay}ms`);

    if (typeof IntersectionObserver === 'undefined') {
      this.renderer.addClass(element, 'reveal-visible');
      return;
    }

    this.observer = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (!entry.isIntersecting) {
            if (!this.revealOnce) {
              this.renderer.removeClass(element, 'reveal-visible');
            }
            continue;
          }

          this.renderer.addClass(element, 'reveal-visible');

          if (this.revealOnce) {
            this.observer?.unobserve(element);
          }
        }
      },
      {
        threshold: 0.2,
        rootMargin: '0px 0px -8% 0px'
      }
    );

    this.observer.observe(element);
  }

  public ngOnDestroy(): void {
    this.observer?.disconnect();
  }
}
