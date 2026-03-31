export class RegistrarUsuarioDto {
  nombres: string;
  apellidos: string;
  documentoIdentidad: string;
  telefono: string | null;
  email: string | null;
  edad: number | null;

  constructor() {
    this.nombres = '';
    this.apellidos = '';
    this.documentoIdentidad = '';
    this.telefono = null;
    this.email = null;
    this.edad = null;
  }
}
