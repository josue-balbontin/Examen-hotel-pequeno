export class Servicio {
  idServicios: number;
  nombreServicio: string | null;
  telefono: string | null;
  encargado: string | null;

  constructor() {
    this.idServicios = 0;
    this.nombreServicio = null;
    this.telefono = null;
    this.encargado = null;
  }
}
