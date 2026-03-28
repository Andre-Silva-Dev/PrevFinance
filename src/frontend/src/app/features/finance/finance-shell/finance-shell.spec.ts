import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { AccountsApiService } from '../../../core/finance/accounts-api.service';
import { InstallmentsApiService } from '../../../core/finance/installments-api.service';
import { TransactionsApiService } from '../../../core/finance/transactions-api.service';

import { FinanceShell } from './finance-shell';

describe('FinanceShell', () => {
  let component: FinanceShell;
  let fixture: ComponentFixture<FinanceShell>;
  let listCalls = 0;
  let createCalls = 0;

  const accountsApiMock = {
    list: () => {
      listCalls += 1;
      return of([]);
    },
    create: () => {
      createCalls += 1;
      return of({ id: 'a1', name: 'Conta', type: 'Checking', initialBalance: 100, currentBalance: 100, effectiveBalance: 100 });
    },
    update: () => of({ id: 'a1', name: 'Conta Nova', type: 'Checking', initialBalance: 100, currentBalance: 100, effectiveBalance: 100 }),
    recalibrate: () => of({ id: 'a1', name: 'Conta Nova', type: 'Checking', initialBalance: 100, currentBalance: 90, effectiveBalance: 90 }),
    listAdjustments: () => of([]),
    delete: () => of(void 0)
  };

  const installmentsApiMock = {
    create: () => of({
      id: 'p1',
      accountId: 'a1',
      totalAmount: 1200,
      installmentCount: 12,
      startDate: '2026-01-31',
      frequency: 'Monthly',
      description: 'Notebook',
      type: 'Expense',
      installments: [
        { id: 't1', number: 1, total: 12, amount: 100, dueOn: '2026-01-31', status: 'Pending' }
      ]
    })
  };

  const transactionsApiMock = {
    update: () => of({
      transactions: [
        {
          id: 't1',
          accountId: 'a1',
          amount: 120,
          dueOn: '2026-02-10',
          description: 'Atualizada',
          status: 'Pending',
          installmentPlanId: 'p1',
          installmentNumber: 1,
          installmentCount: 12
        }
      ]
    })
  };

  const sessionMock = {
    session: () => ({
      user: { userId: 'u1', email: 'user@prevfinance.app' },
      tokens: { accessToken: 'access', refreshToken: 'refresh' }
    })
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FinanceShell],
      providers: [
        provideRouter([]),
        { provide: AccountsApiService, useValue: accountsApiMock },
        { provide: InstallmentsApiService, useValue: installmentsApiMock },
        { provide: TransactionsApiService, useValue: transactionsApiMock },
        { provide: AuthSessionService, useValue: sessionMock }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FinanceShell);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create account when form is valid', () => {
    component['createForm'].setValue({ name: 'Conta Principal', type: 'Checking', initialBalance: 1500 });

    component['createAccount']();

    expect(createCalls).toBeGreaterThan(0);
  });

  it('should reload accounts list', () => {
    component['loadAccounts']();

    expect(listCalls).toBeGreaterThan(0);
  });

  it('should recalibrate account when form is valid', () => {
    component['recalibrateForm'].setValue({ accountId: 'a1', newBalance: 90, reason: 'Ajuste' });

    component['recalibrateBalance']();

    expect(component['feedback']()).toContain('recalibrado');
  });

  it('should create installment plan when form is valid', () => {
    component['installmentForm'].setValue({
      accountId: 'a1',
      totalAmount: 1200,
      installmentCount: 12,
      startDate: '2026-01-31',
      frequency: 'Monthly',
      description: 'Notebook',
      type: 'Expense'
    });

    component['createInstallmentPlan']();

    expect(component['generatedInstallmentPlan']()).not.toBeNull();
  });

  it('should update installment when edit form is valid', () => {
    component['generatedInstallmentPlan'].set({
      id: 'p1',
      accountId: 'a1',
      totalAmount: 1200,
      installmentCount: 12,
      startDate: '2026-01-31',
      frequency: 'Monthly',
      description: 'Notebook',
      type: 'Expense',
      installments: [
        { id: 't1', number: 1, total: 12, amount: 100, dueOn: '2026-01-31', status: 'Pending' }
      ]
    });

    component['editInstallmentForm'].setValue({
      transactionId: 't1',
      amount: 120,
      dueOn: '2026-02-10',
      description: 'Atualizada',
      applyToFutureInSeries: false
    });

    component['updateInstallment']();

    expect(component['generatedInstallmentPlan']()?.installments[0].amount).toBe(120);
  });
});
