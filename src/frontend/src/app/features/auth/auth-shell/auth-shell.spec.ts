import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { AuthApiService } from '../../../core/auth/auth-api.service';
import { AuthSessionService } from '../../../core/auth/auth-session.service';

import { AuthShell } from './auth-shell';

describe('AuthShell', () => {
  let component: AuthShell;
  let fixture: ComponentFixture<AuthShell>;
  let loginCalls = 0;
  let registerCalls = 0;
  let setSessionCalls = 0;

  const authApiMock = {
    register: () => {
      registerCalls += 1;
      return of({
        userId: 'u1',
        email: 'user@prevfinance.app',
        accessToken: 'access',
        refreshToken: 'refresh',
        refreshTokenExpiresAtUtc: new Date().toISOString()
      });
    },
    login: () => {
      loginCalls += 1;
      return of({
        userId: 'u1',
        email: 'user@prevfinance.app',
        accessToken: 'access',
        refreshToken: 'refresh',
        refreshTokenExpiresAtUtc: new Date().toISOString()
      });
    },
    oauthDemoCallback: () =>
      of({
        userId: 'u2',
        email: 'oauth@prevfinance.app',
        accessToken: 'access',
        refreshToken: 'refresh',
        refreshTokenExpiresAtUtc: new Date().toISOString()
      }),
    refresh: () => of(),
    logout: () => of(void 0),
    me: () => of({ userId: 'u1', email: 'user@prevfinance.app' }),
    getProfile: () => of({ userId: 'u1', email: 'user@prevfinance.app', fullName: 'User Name' }),
    updateProfile: () => of({ userId: 'u1', email: 'user@prevfinance.app', fullName: 'User Name' })
  };

  const sessionServiceMock = {
    session: () => null,
    isAuthenticated: () => false,
    setSession: () => {
      setSessionCalls += 1;
    },
    updateTokens: () => undefined,
    clear: () => undefined
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthShell],
      providers: [
        provideRouter([]),
        { provide: AuthApiService, useValue: authApiMock },
        { provide: AuthSessionService, useValue: sessionServiceMock }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthShell);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should submit login when form is valid', () => {
    component['loginForm'].setValue({ email: 'user@prevfinance.app', password: 'StrongPass123' });

    component['submitLogin']();

    expect(loginCalls).toBe(1);
    expect(setSessionCalls).toBe(1);
  });

  it('should submit register when form is valid', () => {
    component['registerForm'].setValue({
      fullName: 'User Name',
      email: 'user@prevfinance.app',
      password: 'StrongPass123'
    });

    component['submitRegister']();

    expect(registerCalls).toBe(1);
    expect(setSessionCalls).toBe(2);
  });
});
