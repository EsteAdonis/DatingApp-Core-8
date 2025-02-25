import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

// CanActivate interface
// Interface that a class can implement to be a guard deciding if a route can be activated. 
// If all guards return true, navigation continues. If any guard returns false, navigation is cancelled. 
// If any guard returns a UrlTree, the current navigation is cancelled and a new navigation begins 
// to the UrlTree returned from the guard.

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService)
  const toastr = inject(ToastrService)

  if (accountService.currentUser()){
    return true;
  } else {
    toastr.error('You shall not pass! Please login first');
    return false;
  }
};
