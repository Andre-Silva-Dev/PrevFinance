import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly appName = 'PrevFinance';

  protected readonly menu = [
    { path: '/', label: 'Inicio' },
    { path: '/auth', label: 'Auth' },
    { path: '/finance', label: 'Finance' },
    { path: '/projection', label: 'Projection' },
    { path: '/debt', label: 'Debt' }
  ];
}
