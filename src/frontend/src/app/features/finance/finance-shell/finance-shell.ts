import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthSessionService } from '../../../core/auth/auth-session.service';
import {
  AccountBalanceAdjustmentResponseDto,
  AccountResponseDto,
  AccountType,
  AccountsApiService
} from '../../../core/finance/accounts-api.service';
import { RevealOnScrollDirective } from '../../../shared/directives/reveal-on-scroll.directive';

@Component({
  selector: 'app-finance-shell',
  imports: [CommonModule, ReactiveFormsModule, RouterLink, RevealOnScrollDirective],
  templateUrl: './finance-shell.html',
  styleUrl: './finance-shell.scss'
})
export class FinanceShell {
  private readonly formBuilder = inject(FormBuilder);
  private readonly accountsApi = inject(AccountsApiService);
  protected readonly sessionService = inject(AuthSessionService);

  protected readonly loading = signal(false);
  protected readonly actionLoading = signal(false);
  protected readonly feedback = signal<string | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly accounts = signal<AccountResponseDto[]>([]);
  protected readonly selectedAccountAdjustments = signal<AccountBalanceAdjustmentResponseDto[]>([]);

  protected readonly accountTypes: AccountType[] = ['Checking', 'Savings', 'CreditCard', 'Wallet'];

  protected readonly createForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    type: ['Checking' as AccountType, [Validators.required]],
    initialBalance: [0, [Validators.required]]
  });

  protected readonly renameForm = this.formBuilder.group({
    accountId: ['', [Validators.required]],
    name: ['', [Validators.required, Validators.minLength(2)]]
  });

  protected readonly recalibrateForm = this.formBuilder.group({
    accountId: ['', [Validators.required]],
    newBalance: [0, [Validators.required]],
    reason: ['', [Validators.required, Validators.minLength(3)]]
  });

  constructor() {
    if (this.sessionService.session()) {
      this.loadAccounts();
    }
  }

  protected loadAccounts(): void {
    if (!this.sessionService.session()) {
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.accountsApi
      .list()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (accounts) => this.accounts.set(accounts),
        error: () => this.error.set('Nao foi possivel carregar suas contas no momento.')
      });
  }

  protected createAccount(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.actionLoading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    const payload = this.createForm.getRawValue();
    this.accountsApi
      .create({
        name: payload.name ?? '',
        type: (payload.type ?? 'Checking') as AccountType,
        initialBalance: Number(payload.initialBalance ?? 0)
      })
      .pipe(finalize(() => this.actionLoading.set(false)))
      .subscribe({
        next: (created) => {
          this.accounts.set([created, ...this.accounts()]);
          this.feedback.set('Conta criada com sucesso.');
          this.createForm.patchValue({ name: '', initialBalance: 0 });
        },
        error: () => this.error.set('Falha ao criar conta. Revise os dados e tente novamente.')
      });
  }

  protected preloadRename(account: AccountResponseDto): void {
    this.renameForm.patchValue({ accountId: account.id, name: account.name });
  }

  protected renameAccount(): void {
    if (this.renameForm.invalid) {
      this.renameForm.markAllAsTouched();
      return;
    }

    const payload = this.renameForm.getRawValue();
    this.actionLoading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.accountsApi
      .update(payload.accountId ?? '', { name: payload.name ?? '' })
      .pipe(finalize(() => this.actionLoading.set(false)))
      .subscribe({
        next: (updated) => {
          this.accounts.set(this.accounts().map((item) => (item.id === updated.id ? updated : item)));
          this.feedback.set('Conta atualizada com sucesso.');
        },
        error: () => this.error.set('Falha ao atualizar conta. Verifique a selecao atual.')
      });
  }

  protected deleteAccount(accountId: string): void {
    this.actionLoading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.accountsApi
      .delete(accountId)
      .pipe(finalize(() => this.actionLoading.set(false)))
      .subscribe({
        next: () => {
          this.accounts.set(this.accounts().filter((item) => item.id !== accountId));
          if (this.recalibrateForm.getRawValue().accountId === accountId) {
            this.recalibrateForm.patchValue({ accountId: '' });
            this.selectedAccountAdjustments.set([]);
          }
          this.feedback.set('Conta removida com sucesso.');
        },
        error: () => this.error.set('Nao foi possivel remover a conta selecionada.')
      });
  }

  protected preloadRecalibration(account: AccountResponseDto): void {
    this.recalibrateForm.patchValue({
      accountId: account.id,
      newBalance: account.currentBalance,
      reason: ''
    });

    this.loadAdjustmentHistory(account.id);
  }

  protected recalibrateBalance(): void {
    if (this.recalibrateForm.invalid) {
      this.recalibrateForm.markAllAsTouched();
      return;
    }

    const payload = this.recalibrateForm.getRawValue();
    const accountId = payload.accountId ?? '';

    this.actionLoading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.accountsApi
      .recalibrate(accountId, {
        newBalance: Number(payload.newBalance ?? 0),
        reason: payload.reason ?? ''
      })
      .pipe(finalize(() => this.actionLoading.set(false)))
      .subscribe({
        next: (updated) => {
          this.accounts.set(this.accounts().map((item) => (item.id === updated.id ? updated : item)));
          this.feedback.set('Saldo recalibrado com sucesso.');
          this.loadAdjustmentHistory(updated.id);
          this.recalibrateForm.patchValue({ reason: '' });
        },
        error: () => this.error.set('Falha ao recalibrar saldo. Verifique a conta e o motivo do ajuste.')
      });
  }

  protected loadAdjustmentHistory(accountId: string): void {
    if (!accountId) {
      this.selectedAccountAdjustments.set([]);
      return;
    }

    this.accountsApi.listAdjustments(accountId).subscribe({
      next: (items) => this.selectedAccountAdjustments.set(items),
      error: () => this.error.set('Nao foi possivel carregar o historico de ajustes desta conta.')
    });
  }

  protected formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      minimumFractionDigits: 2
    }).format(value);
  }

  protected formatDateTime(value: string): string {
    return new Intl.DateTimeFormat('pt-BR', {
      dateStyle: 'short',
      timeStyle: 'short'
    }).format(new Date(value));
  }

}
