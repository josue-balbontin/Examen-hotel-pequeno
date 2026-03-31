import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Servicio } from '../../Models/servicio.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioApiService {
  private apiUrl = environment.apiUrl + '/ServicioControlador';

  constructor(private http: HttpClient) {}

  private mapearServicio(item: any): Servicio {
    const servicio = new Servicio();
    servicio.idServicios = item.idServicios ?? 0;
    servicio.nombreServicio = item.nombreServicio ?? null;
    servicio.telefono = item.telefono ?? null;
    servicio.encargado = item.encargado ?? null;
    return servicio;
  }

  obtenerContactos() {
    return this.http.get<any>(`${this.apiUrl}/Contactos`).pipe(
      map((data) => {
        if (!Array.isArray(data)) {
          return data;
        }

        return data.map((item) => this.mapearServicio(item));
      })
    );
  }
}
