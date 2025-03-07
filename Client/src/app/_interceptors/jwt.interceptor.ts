import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';

// Interceptors
// Interceptors are middleware that allows common patterns around retrying, caching, logging, 
// and authentication to be abstracted away from individual requests.

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountServer = inject(AccountService);

  if (accountServer.currentUser()) {
    req = req.clone( {
      setHeaders: {
        Authorization: `Bearer ${accountServer.currentUser()?.token}`
      }
    })
  }

  return next(req);
};
