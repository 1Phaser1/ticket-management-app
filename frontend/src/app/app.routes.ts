import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'tickets'
  },
  {
    path: 'tickets',
    loadComponent: () =>
      import('./pages/ticket-list/ticket-list.component').then((m) => m.TicketListComponent)
  },
  {
    path: 'tickets/new',
    loadComponent: () =>
      import('./pages/ticket-form/ticket-form.component').then((m) => m.TicketFormComponent)
  },
  {
    path: 'tickets/:id',
    loadComponent: () =>
      import('./pages/ticket-detail/ticket-detail.component').then((m) => m.TicketDetailComponent)
  },
  {
    path: 'tickets/:id/edit',
    loadComponent: () =>
      import('./pages/ticket-form/ticket-form.component').then((m) => m.TicketFormComponent)
  },
  {
    path: '**',
    redirectTo: 'tickets'
  }
];
