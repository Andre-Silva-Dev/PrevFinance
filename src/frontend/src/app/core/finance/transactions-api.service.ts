import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { TransactionStatus } from './installments-api.service';

export interface UpdatedTransactionResponseDto {
  id: string;
  accountId: string;
  amount: number;
  dueOn: string;
  description: string;
  status: TransactionStatus;
  installmentPlanId: string | null;
  installmentNumber: number | null;
  installmentCount: number | null;
}

export interface UpdatedTransactionBatchResponseDto {
  transactions: UpdatedTransactionResponseDto[];
}

@Injectable({ providedIn: 'root' })
export class TransactionsApiService {
  private readonly baseUrl = '/api/transactions';
  private readonly http = inject(HttpClient);

  update(
    transactionId: string,
    payload: { amount?: number; dueOn?: string; description?: string; applyToFutureInSeries: boolean }
  ): Observable<UpdatedTransactionBatchResponseDto> {
    return this.http.patch<UpdatedTransactionBatchResponseDto>(`${this.baseUrl}/${transactionId}`, payload);
  }
}
