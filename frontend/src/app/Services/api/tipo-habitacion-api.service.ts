import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { TipoHabitacion } from '../../Models/tipo-habitacion.model';

@Injectable({
  providedIn: 'root'
})
export class TipoHabitacionApiService {
  private apiUrl = environment.apiUrl + '/TipoHabitacionControlador';

  constructor(private http: HttpClient) {}

  private mapearTipoHabitacion(item: any): TipoHabitacion {
    const tipoHabitacion = new TipoHabitacion();
    tipoHabitacion.idTipoHabitaciones = item.idTipoHabitaciones ?? 0;
    tipoHabitacion.nombre = item.nombre ?? null;
    tipoHabitacion.capacidad = item.capacidad ?? null;
    tipoHabitacion.precioReferencia = item.precioReferencia ?? null;
    tipoHabitacion.descripcion = item.descripcion ?? null;
    return tipoHabitacion;
  }

  obtenerOpciones() {
    return this.http
      .get<any[]>(`${this.apiUrl}/Opciones`)
      .pipe(map((data) => data.map((item) => this.mapearTipoHabitacion(item))));
  }
}
