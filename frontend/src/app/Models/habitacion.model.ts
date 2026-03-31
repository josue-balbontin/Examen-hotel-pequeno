import { TipoHabitacion } from './tipo-habitacion.model';

export class Habitacion {
  idHabitaciones: number = 0;
  numeroHabitacion: string = '';
  idTipoHabitacion: number = 0;
  idTipoHabitacionNavigation?: TipoHabitacion | null = null;
}
