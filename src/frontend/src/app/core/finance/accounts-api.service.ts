import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export type AccountType = 'Checking' | 'Savings' | 'CreditCard' | 'Wallet';

export interface AccountResponseDto {
  id: string;
  name: string;
  type: AccountType;
  initialBalance: number;
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

  delete(accountId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${accountId}`);
  }
}
