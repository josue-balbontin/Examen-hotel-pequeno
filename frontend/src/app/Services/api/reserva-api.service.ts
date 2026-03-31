import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { CrearReservaDto } from '../../Models/crear-reserva.dto';
import { Reserva } from '../../Models/reserva.model';

@Injectable({
  providedIn: 'root'
})
export class ReservaApiService {
  private apiUrl = environment.apiUrl + '/ReservaControlador';

  constructor(private http: HttpClient) { }

  private mapearReserva(item: any): Reserva {
    const reserva = new Reserva();
    reserva.idReservas = item.idReservas ?? 0;
    reserva.idHabitaciones = item.idHabitaciones ?? null;
    reserva.fechaIngreso = item.fechaIngreso ?? null;
    reserva.fechaSalida = item.fechaSalida ?? null;
    reserva.idEstados = item.idEstados ?? null;
    reserva.fechaCheckin = item.fechaCheckin ?? null;
    reserva.fechaCheckout = item.fechaCheckout ?? null;
    reserva.cargoCheckout = item.cargoCheckout ?? null;
    return reserva;
  }

  crearReserva(payload: CrearReservaDto) {
    const envio = {
      idsUsuarios: payload.idsUsuarios,
      idHabitacion: payload.idHabitacion,
      fechaIngreso: payload.fechaIngreso,
      fechaSalida: payload.fechaSalida
    };

    return this.http.post<any>(`${this.apiUrl}/CrearReserva`, envio);
  }

  obtenerReservas() {
    return this.http
      .get<any[]>(`${this.apiUrl}/ObtenerReservas`)
      .pipe(
        map(
          (data) => data.map((item) => this.mapearReserva(item))
        )
      );
  }

  registrarCheckIn(idReserva: number) {
    return this.http.put<any>(`${environment.apiUrl}/checkin/${idReserva}`, {});
  }

  registrarCheckOut(idReserva: number) {
    return this.http.put<any>(`${environment.apiUrl}/checkout/${idReserva}`, {});
  }

  cancelarReserva(idReserva: number) {
    return this.http.put<any>(`${environment.apiUrl}/cancelar/${idReserva}`, {});
  }

  disponibilidadHabitaciones(fechaIngreso: string, fechaSalida: string) {
    return this.http.get<any[]>(`${this.apiUrl}/Disponibilidad?ingreso=${fechaIngreso}&salida=${fechaSalida}`);
  }

}
