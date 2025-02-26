import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

// Interceptors
// Interceptors are generally functions which you can run for each request, 
// and have broad capabilities to affect the contents and overall flow of requests and responses. 
// https://angular.dev/guide/http/interceptors

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toast = inject(ToastrService);
  return next(req).pipe(
    catchError((error) => {
      switch(error.status) {
        case 400:
          if (error.error.errors) {
            const modalStateErrors = [];
            for (const key in error.error.errors) {
              if (error.error.errors[key]) {
                modalStateErrors.push(error.error.errors[key]);
              }
            }
            throw modalStateErrors.flat();
          } else {
            toast.error(error.statusText, error.status);
          }
          break;
        case 401:
          toast.error(error.statusText, error.status);
          break;
        case 404:
          router.navigateByUrl('/not-found'); 
          break;
        case 500:
          const navigationExtras: NavigationExtras = {state: {error: error.error}};
          router.navigateByUrl('/server-error', navigationExtras);
          break;
        default:
          toast.error('Something unexpected went wrong');
          break;
      }
      throw error;
    })
  );
};
