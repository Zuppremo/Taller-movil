import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login-form',
    loadComponent: () => import('./views/login-form/login-form.page').then((m) => m.LoginFormPage),
  },
  {
    path: '',
    redirectTo: 'login-form',
    pathMatch: 'full',
  },
  {
    path: 'home',
    loadComponent: () => import('./home/home.page').then( m => m.HomePage)
  },
];
