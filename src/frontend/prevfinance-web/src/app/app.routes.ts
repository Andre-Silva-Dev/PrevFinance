import { Routes } from '@angular/router';
import { AuthShell } from './features/auth/auth-shell/auth-shell';
import { DebtShell } from './features/debt/debt-shell/debt-shell';
import { FinanceShell } from './features/finance/finance-shell/finance-shell';
import { HomePage } from './features/home/home-page/home-page';
import { NotFoundPage } from './features/not-found/not-found-page/not-found-page';
import { ProjectionShell } from './features/projection/projection-shell/projection-shell';

export const routes: Routes = [
	{ path: '', component: HomePage },
	{ path: 'auth', component: AuthShell },
	{ path: 'finance', component: FinanceShell },
	{ path: 'projection', component: ProjectionShell },
	{ path: 'debt', component: DebtShell },
	{ path: '**', component: NotFoundPage }
];
