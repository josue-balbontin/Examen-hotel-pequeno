import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { RegistrarUsuarioDto } from '../../Models/registrar-usuario.dto';
import { Usuario } from '../../Models/usuario.model';

@Injectable({
  providedIn: 'root'
})
export class UsuarioApiService {
  private apiUrl = environment.apiUrl + '/UsuarioControlador';

  constructor(private http: HttpClient) {}

  private mapearUsuario(item: any): Usuario {
    const usuario = new Usuario();
    usuario.idUsuarios = item.idUsuarios ?? 0;
    usuario.nombres = item.nombres ?? null;
    usuario.apellidos = item.apellidos ?? null;
    usuario.documentoIdentidad = item.documentoIdentidad ?? '';
    usuario.telefono = item.telefono ?? null;
    usuario.email = item.email ?? null;
    usuario.edad = item.edad ?? null;
    return usuario;
  }

  obtenerUsuarios() {
    return this.http
      .get<any[]>(`${this.apiUrl}/ObtenerUsuarios`)
      .pipe(map((data) => data.map((item) => this.mapearUsuario(item))));
  }

  registrarUsuario(payload: RegistrarUsuarioDto) {
    const envio = {
      nombres: payload.nombres,
      apellidos: payload.apellidos,
      documentoIdentidad: payload.documentoIdentidad,
      telefono: payload.telefono,
      email: payload.email,
      edad: payload.edad
    };

    return this.http.post<any>(`${this.apiUrl}/RegistrarUsuario`, envio);
  }
}
