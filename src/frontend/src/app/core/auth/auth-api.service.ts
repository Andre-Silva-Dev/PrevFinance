import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export interface AuthResponseDto {
  userId: string;
  email: string;
  accessToken: string;
  refreshToken: string;
  refreshTokenExpiresAtUtc: string;
}

export interface ProfileResponseDto {
  userId: string;
  email: string;
  fullName: string;
}

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly baseUrl = '/api';
  private readonly http = inject(HttpClient);

  register(payload: { email: string; fullName: string; password: string }): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/register`, payload);
  }

  login(payload: { email: string; password: string }): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/login`, payload);
  }

  oauthDemoCallback(payload: { code: string }): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/oauth2/demo/callback`, payload);
  }

  refresh(refreshToken: string): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/refresh`, { refreshToken });
  }

  logout(refreshToken: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/auth/logout`, { refreshToken });
  }

  me(): Observable<{ userId: string; email: string }> {
    return this.http.get<{ userId: string; email: string }>(`${this.baseUrl}/auth/me`);
  }

  getProfile(): Observable<ProfileResponseDto> {
    return this.http.get<ProfileResponseDto>(`${this.baseUrl}/profile/me`);
  }

  updateProfile(payload: { fullName: string }): Observable<ProfileResponseDto> {
    return this.http.put<ProfileResponseDto>(`${this.baseUrl}/profile/me`, payload);
  }
}
