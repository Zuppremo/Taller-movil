import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'home',
    loadComponent: () => import('./views/login-form/login-form.page').then((m) => m.LoginFormPage),
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'login-form',
    loadComponent: () => import('./views/login-form/login-form.page').then( m => m.LoginFormPage)
  },
];
