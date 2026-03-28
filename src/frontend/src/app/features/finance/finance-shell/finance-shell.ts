import { Component } from '@angular/core';
import { RevealOnScrollDirective } from '../../../shared/directives/reveal-on-scroll.directive';

@Component({
  selector: 'app-finance-shell',
  imports: [RevealOnScrollDirective],
  templateUrl: './finance-shell.html',
  styleUrl: './finance-shell.scss',
})
export class FinanceShell {

}
