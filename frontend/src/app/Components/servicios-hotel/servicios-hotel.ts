import { Component, OnInit, ChangeDetectorRef, signal, WritableSignal } from '@angular/core';
import { TablaGenerica } from '../tabla-generica/tabla-generica';
import { ColumnaTabla, AccionTabla } from '../../Models/tabla-generica/tabla-generica';
import { ServicioApiService } from '../../Services/api/servicio-api.service';
import { Servicio } from '../../Models/servicio.model';
import { PantallaCargaComponent } from '../pantallas_avisos/pantalla-carga/pantalla-carga.component';

@Component({
  selector: 'app-servicios-hotel',
  standalone: true,
  imports: [TablaGenerica, PantallaCargaComponent],
  templateUrl: './servicios-hotel.html',
  styleUrl: './servicios-hotel.css',
})
export class ServiciosHotel implements OnInit {

  servicios: Servicio[] = [];
  cargando: WritableSignal<boolean> = signal<boolean>(false);

  columnas: ColumnaTabla[] = [
    { titulo: 'Servicio', contenido: 'nombreServicio' },
    { titulo: 'Encargado', contenido: 'encargado' },
    { titulo: 'Teléfono', contenido: 'telefono' }
  ];

  acciones: AccionTabla[] = [];

  constructor(private servicioApi: ServicioApiService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.cargarServicios();
  }

  cargarServicios(): void {
    this.cargando.set(true);
    this.servicioApi.obtenerContactos().subscribe({
      next: (data) => {
        this.servicios = [...data];
        this.cargando.set(false);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al cargar servicios:', err);
        this.servicios = [];
        this.cargando.set(false);
        this.cdr.detectChanges();
      }
    });
  }

}
