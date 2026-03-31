export class TipoHabitacion {
  idTipoHabitaciones: number;
  nombre: string | null;
  capacidad: number | null;
  precioReferencia: number | null;
  descripcion: string | null;

  constructor() {
    this.idTipoHabitaciones = 0;
    this.nombre = null;
    this.capacidad = null;
    this.precioReferencia = null;
    this.descripcion = null;
  }
}
