export class Reserva {
  idReservas: number;
  idHabitaciones: number | null;
  fechaIngreso: string | null;
  fechaSalida: string | null;
  idEstados: number | null;
  fechaCheckin: string | null;
  fechaCheckout: string | null;
  cargoCheckout: number | null;

  constructor() {
    this.idReservas = 0;
    this.idHabitaciones = null;
    this.fechaIngreso = null;
    this.fechaSalida = null;
    this.idEstados = null;
    this.fechaCheckin = null;
    this.fechaCheckout = null;
    this.cargoCheckout = null;
  }
}
