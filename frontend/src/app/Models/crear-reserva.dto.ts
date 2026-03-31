export class CrearReservaDto {
  idsUsuarios: number[];
  idHabitacion: number;
  fechaIngreso: string;
  fechaSalida: string;

  constructor() {
    this.idsUsuarios = [];
    this.idHabitacion = 0;
    this.fechaIngreso = '';
    this.fechaSalida = '';
  }
}
