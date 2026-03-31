import { Routes } from '@angular/router';
import { Reservas } from './Components/reservas/reservas';
import { ServiciosHotel } from './Components/servicios-hotel/servicios-hotel';
import { Usuarios } from './Components/usuarios/usuarios';

export const routes: Routes = [
  { path: '', redirectTo: 'reservas', pathMatch: 'full' },
  { path: 'reservas', component: Reservas },
  { path: 'servicios-hotel', component: ServiciosHotel },
  { path: 'usuarios', component: Usuarios },
];
