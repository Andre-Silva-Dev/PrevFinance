import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export type AccountType = 'Checking' | 'Savings' | 'CreditCard' | 'Wallet';

export interface AccountResponseDto {
  id: string;
  name: string;
  type: AccountType;
  initialBalance: number;
  currentBalance: number;
  effectiveBalance: number;
}

export interface AccountBalanceAdjustmentResponseDto {
  id: string;
  accountId: string;
  previousBalance: number;
  newBalance: number;
  reason: string;
  adjustedAtUtc: string;
}

@Injectable({ providedIn: 'root' })
export class AccountsApiService {
  private readonly baseUrl = '/api/accounts';
  private readonly http = inject(HttpClient);

  list(): Observable<AccountResponseDto[]> {
    return this.http.get<AccountResponseDto[]>(`${this.baseUrl}/`);
  }

  create(payload: { name: string; type: AccountType; initialBalance: number }): Observable<AccountResponseDto> {
    return this.http.post<AccountResponseDto>(`${this.baseUrl}/`, payload);
  }

  update(accountId: string, payload: { name: string }): Observable<AccountResponseDto> {
    return this.http.put<AccountResponseDto>(`${this.baseUrl}/${accountId}`, payload);
  }

  recalibrate(accountId: string, payload: { newBalance: number; reason: string }): Observable<AccountResponseDto> {
    return this.http.post<AccountResponseDto>(`${this.baseUrl}/${accountId}/recalibrate`, payload);
  }

  listAdjustments(accountId: string): Observable<AccountBalanceAdjustmentResponseDto[]> {
    return this.http.get<AccountBalanceAdjustmentResponseDto[]>(`${this.baseUrl}/${accountId}/adjustments`);
  }

  delete(accountId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${accountId}`);
  }
}
