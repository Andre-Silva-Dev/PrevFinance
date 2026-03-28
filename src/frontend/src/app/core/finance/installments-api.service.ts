import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export type InstallmentFrequency = 'Monthly' | 'Weekly';
export type TransactionType = 'Income' | 'Expense';
export type TransactionStatus = 'Pending' | 'Paid' | 'Cancelled' | 'Overdue';

export interface InstallmentTransactionResponseDto {
  id: string;
  number: number;
  total: number;
  amount: number;
  dueOn: string;
  status: TransactionStatus;
}

export interface InstallmentPlanResponseDto {
  id: string;
  accountId: string;
  totalAmount: number;
  installmentCount: number;
  startDate: string;
  frequency: InstallmentFrequency;
  description: string;
  type: TransactionType;
  installments: InstallmentTransactionResponseDto[];
}

@Injectable({ providedIn: 'root' })
export class InstallmentsApiService {
  private readonly baseUrl = '/api/installments';
  private readonly http = inject(HttpClient);

  create(payload: {
    accountId: string;
    totalAmount: number;
    installmentCount: number;
    startDate: string;
    frequency: InstallmentFrequency;
    description: string;
    type: TransactionType;
  }): Observable<InstallmentPlanResponseDto> {
    return this.http.post<InstallmentPlanResponseDto>(`${this.baseUrl}/`, payload);
  }
}
