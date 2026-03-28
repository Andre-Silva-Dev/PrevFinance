import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { AccountsApiService } from '../../../core/finance/accounts-api.service';

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
      return of({ id: 'a1', name: 'Conta', type: 'Checking', initialBalance: 100, currentBalance: 100 });
    },
    update: () => of({ id: 'a1', name: 'Conta Nova', type: 'Checking', initialBalance: 100, currentBalance: 100 }),
    recalibrate: () => of({ id: 'a1', name: 'Conta Nova', type: 'Checking', initialBalance: 100, currentBalance: 90 }),
    listAdjustments: () => of([]),
    delete: () => of(void 0)
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
});
