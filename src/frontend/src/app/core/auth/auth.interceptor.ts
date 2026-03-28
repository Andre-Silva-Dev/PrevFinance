import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthApiService } from './auth-api.service';
import { AuthSessionService } from './auth-session.service';

let refreshInFlight = false;

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const sessionService = inject(AuthSessionService);
  const authApi = inject(AuthApiService);
  const token = sessionService.accessToken;

  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const canRefresh =
        error.status === 401 &&
        !req.url.includes('/auth/login') &&
        !req.url.includes('/auth/register') &&
        !req.url.includes('/auth/refresh');

      if (!canRefresh || refreshInFlight) {
        return throwError(() => error);
      }

      const refreshToken = sessionService.session()?.tokens.refreshToken;
      if (!refreshToken) {
        sessionService.clear();
        return throwError(() => error);
      }

      refreshInFlight = true;

      return authApi.refresh(refreshToken).pipe(
        switchMap((response) => {
          sessionService.setSession({
            user: { userId: response.userId, email: response.email },
            tokens: {
              accessToken: response.accessToken,
              refreshToken: response.refreshToken
            }
          });

          refreshInFlight = false;
          const retried = req.clone({ setHeaders: { Authorization: `Bearer ${response.accessToken}` } });
          return next(retried);
        }),
        catchError((refreshError) => {
          refreshInFlight = false;
          sessionService.clear();
          return throwError(() => refreshError);
        })
      );
    })
  );
};
