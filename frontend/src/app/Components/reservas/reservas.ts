import { Component, OnInit, ChangeDetectorRef, signal, WritableSignal } from '@angular/core';
import { TablaGenerica } from '../tabla-generica/tabla-generica';
import { ColumnaTabla, AccionTabla } from '../../Models/tabla-generica/tabla-generica';
import { ReservaApiService } from '../../Services/api/reserva-api.service';
import { Reserva } from '../../Models/reserva.model';
import { EstadoReserva } from '../../Models/estado-reserva.enum';
import { PantallaCargaComponent } from '../pantallas_avisos/pantalla-carga/pantalla-carga.component';
import { MostrarerrorComponent } from '../pantallas_avisos/mostrarerror/mostrarerror.component';
import { CrearReserva } from './crear-reserva/crear-reserva';

@Component({
  selector: 'app-reservas',
  standalone: true,
  imports: [TablaGenerica, PantallaCargaComponent, MostrarerrorComponent, CrearReserva],
  templateUrl: './reservas.html',
  styleUrl: './reservas.css',
})
export class Reservas implements OnInit {

  reservas: any[] = [];
  cargando: WritableSignal<boolean> = signal<boolean>(false);
  error: WritableSignal<boolean> = signal<boolean>(false);
  mostrarModalCrear: WritableSignal<boolean> = signal<boolean>(false);
  mensajeError: string = '';

  columnas: ColumnaTabla[] = [
    { titulo: 'ID Reserva', contenido: 'idReservas' },
    { titulo: 'ID Habitación', contenido: 'idHabitaciones' },
    { titulo: 'Fecha Ingreso', contenido: 'fechaIngreso' },
    { titulo: 'Fecha Salida', contenido: 'fechaSalida' },
    { titulo: 'Fecha check-in', contenido: 'fechaCheckin' },
    { titulo: 'Fecha check-out', contenido: 'fechaCheckout' },
    { titulo: 'Cargo Check-out', contenido: 'cargoCheckout' },
    { titulo: 'Estado', contenido: 'nombreEstado' }
  ];

  acciones: AccionTabla[] = [
    {
      nombre: 'checkin',
      etiqueta: 'Check-In',
      color: '#10b981',
      colorBorde: '#10b981',
      mostrar: (item: any) => item.idEstados === EstadoReserva.Reservado
    },
    {
      nombre: 'checkout',
      etiqueta: 'Check-Out',
      color: '#f59e0b',
      colorBorde: '#f59e0b',
      mostrar: (item: any) => item.idEstados === EstadoReserva.Ocupado
    },
    {
      nombre: 'cancelar',
      etiqueta: 'Cancelar',
      color: '#ef4444',
      colorBorde: '#ef4444',
      mostrar: (item: any) => item.idEstados === EstadoReserva.Reservado
    }
  ];

  constructor(private reservaApi: ReservaApiService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.cargarReservas();
  }

  cargarReservas(): void {
    this.cargando.set(true);
    this.reservaApi.obtenerReservas().subscribe({
      next: (data) => {

        const datasort = data.sort((a, b) => {
          if (!a.fechaIngreso) return 1;
          if (!b.fechaIngreso) return -1;
          return new Date(a.fechaIngreso).getTime() - new Date(b.fechaIngreso).getTime();
        });

        this.reservas = datasort.map(res => ({
          ...res,
          nombreEstado: res.idEstados ? EstadoReserva[res.idEstados as unknown as EstadoReserva] : 'Desconocido'
        }));

        this.cargando.set(false);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al cargar reservas:', err);
        this.reservas = [];
        this.cargando.set(false);
        this.cdr.detectChanges();
      }
    });
  }

  manejarAccion(event: { nombre: string; item: any }): void {
    const id = event.item.idReservas;
    this.cargando.set(true);

    let requestObservable;

    switch (event.nombre) {
      case 'checkin':
        requestObservable = this.reservaApi.registrarCheckIn(id);
        break;
      case 'checkout':
        requestObservable = this.reservaApi.registrarCheckOut(id);
        break;
      case 'cancelar':
        requestObservable = this.reservaApi.cancelarReserva(id);
        break;
    }

    if (requestObservable) {
      requestObservable.subscribe({
        next: () => {
          this.cargarReservas();
        },
        error: (err) => {
          this.cargando.set(false);
          this.mensajeError = err.error?.message || "Ocurrió un error al procesar la acción.";
          this.error.set(true);
          this.cdr.detectChanges();
        }
      });
    }
  }

  abrirModalCrear(): void {
    this.mostrarModalCrear.set(true);
  }

  cerrarModalCrear(): void {
    this.mostrarModalCrear.set(false);
  }

  onReservaCreada(): void {
    this.cerrarModalCrear();
    this.cargarReservas();
  }
} 
