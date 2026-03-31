export class Usuario {
  idUsuarios: number;
  nombres: string | null;
  apellidos: string | null;
  documentoIdentidad: string;
  telefono: string | null;
  email: string | null;
  edad: number | null;

  constructor() {
    this.idUsuarios = 0;
    this.nombres = null;
    this.apellidos = null;
    this.documentoIdentidad = '';
    this.telefono = null;
    this.email = null;
    this.edad = null;
  }
}
