import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthApiService, AuthResponseDto, ProfileResponseDto } from '../../../core/auth/auth-api.service';
import { AuthSessionService } from '../../../core/auth/auth-session.service';
import { RevealOnScrollDirective } from '../../../shared/directives/reveal-on-scroll.directive';

@Component({
  selector: 'app-auth-shell',
  imports: [CommonModule, ReactiveFormsModule, RouterLink, RevealOnScrollDirective],
  templateUrl: './auth-shell.html',
  styleUrl: './auth-shell.scss'
})
export class AuthShell {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authApi = inject(AuthApiService);
  protected readonly sessionService = inject(AuthSessionService);

  protected readonly mode = signal<'login' | 'register'>('login');
  protected readonly loading = signal(false);
  protected readonly busyOAuth = signal(false);
  protected readonly feedback = signal<string | null>(null);
  protected readonly error = signal<string | null>(null);
  protected readonly profile = signal<ProfileResponseDto | null>(null);
  protected readonly userEmail = computed(() => this.sessionService.session()?.user.email ?? null);

  protected readonly loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  protected readonly registerForm = this.formBuilder.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  protected readonly profileForm = this.formBuilder.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]]
  });

  constructor() {
    if (this.sessionService.session()) {
      this.loadProfile();
    }
  }

  protected switchMode(mode: 'login' | 'register'): void {
    this.mode.set(mode);
    this.feedback.set(null);
    this.error.set(null);
  }

  protected submitLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const payload = this.loginForm.getRawValue();
    this.loading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.authApi
      .login({ email: payload.email ?? '', password: payload.password ?? '' })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          this.persistSession(response);
          this.feedback.set('Login realizado com sucesso.');
          this.loadProfile();
        },
        error: () => this.error.set('Falha no login. Verifique e-mail e senha.')
      });
  }

  protected submitRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const payload = this.registerForm.getRawValue();
    this.loading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.authApi
      .register({
        email: payload.email ?? '',
        fullName: payload.fullName ?? '',
        password: payload.password ?? ''
      })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          this.persistSession(response);
          this.feedback.set('Cadastro concluido e sessao iniciada.');
          this.profileForm.patchValue({ fullName: payload.fullName ?? '' });
          this.loadProfile();
        },
        error: () => this.error.set('Falha no cadastro. Revise os campos e tente novamente.')
      });
  }

  protected loginWithDemoOAuth(): void {
    this.busyOAuth.set(true);
    this.feedback.set(null);
    this.error.set(null);

    const code = this.buildDemoCode();
    this.authApi
      .oauthDemoCallback({ code })
      .pipe(finalize(() => this.busyOAuth.set(false)))
      .subscribe({
        next: (response) => {
          this.persistSession(response);
          this.feedback.set('OAuth2 concluido com sucesso no provedor demo.');
          this.loadProfile();
        },
        error: () => this.error.set('Falha no OAuth2 demo. Tente novamente.')
      });
  }

  protected updateProfile(): void {
    if (!this.sessionService.session()) {
      this.error.set('Faca login para atualizar o perfil.');
      return;
    }

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.feedback.set(null);
    this.error.set(null);

    this.authApi
      .updateProfile({ fullName: this.profileForm.getRawValue().fullName ?? '' })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (profile) => {
          this.profile.set(profile);
          this.feedback.set('Perfil atualizado com sucesso.');
        },
        error: () => this.error.set('Nao foi possivel atualizar o perfil.')
      });
  }

  protected logout(): void {
    const refreshToken = this.sessionService.session()?.tokens.refreshToken;
    if (!refreshToken) {
      this.sessionService.clear();
      return;
    }

    this.authApi.logout(refreshToken).subscribe({
      next: () => this.clearSessionState(),
      error: () => this.clearSessionState()
    });
  }

  private loadProfile(): void {
    this.authApi.getProfile().subscribe({
      next: (profile) => {
        this.profile.set(profile);
        this.profileForm.patchValue({ fullName: profile.fullName });
      },
      error: () => {
        this.profile.set(null);
      }
    });
  }

  private persistSession(response: AuthResponseDto): void {
    this.sessionService.setSession({
      user: { userId: response.userId, email: response.email },
      tokens: { accessToken: response.accessToken, refreshToken: response.refreshToken }
    });
  }

  private clearSessionState(): void {
    this.sessionService.clear();
    this.profile.set(null);
    this.feedback.set('Sessao encerrada.');
  }

  private buildDemoCode(): string {
    const payload = {
      Subject: `demo-${Date.now()}`,
      Email: `demo.${Date.now()}@prevfinance.app`,
      FullName: 'Usuario Demo OAuth2'
    };

    return btoa(JSON.stringify(payload));
  }

}
