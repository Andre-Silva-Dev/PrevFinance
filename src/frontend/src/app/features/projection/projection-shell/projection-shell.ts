import { Component } from '@angular/core';
import { RevealOnScrollDirective } from '../../../shared/directives/reveal-on-scroll.directive';

@Component({
  selector: 'app-projection-shell',
  imports: [RevealOnScrollDirective],
  templateUrl: './projection-shell.html',
  styleUrl: './projection-shell.scss',
})
export class ProjectionShell {

}
