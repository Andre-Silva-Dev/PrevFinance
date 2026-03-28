import { Injectable, signal } from '@angular/core';

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

export interface AuthUser {
  userId: string;
  email: string;
}

export interface AuthSession {
  user: AuthUser;
  tokens: AuthTokens;
}

const STORAGE_KEY = 'prevfinance.session';

@Injectable({ providedIn: 'root' })
export class AuthSessionService {
  readonly session = signal<AuthSession | null>(this.loadSession());

  readonly isAuthenticated = signal<boolean>(!!this.session());

  get accessToken(): string | null {
    return this.session()?.tokens.accessToken ?? null;
  }

  setSession(session: AuthSession): void {
    this.session.set(session);
    this.isAuthenticated.set(true);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
  }

  updateTokens(tokens: AuthTokens): void {
    const current = this.session();
    if (!current) {
      return;
    }

    const updated = { ...current, tokens };
    this.session.set(updated);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(updated));
  }

  clear(): void {
    this.session.set(null);
    this.isAuthenticated.set(false);
    localStorage.removeItem(STORAGE_KEY);
  }

  private loadSession(): AuthSession | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) {
      return null;
    }

    try {
      const parsed = JSON.parse(raw) as AuthSession;
      return parsed?.tokens?.accessToken ? parsed : null;
    } catch {
      return null;
    }
  }
}
