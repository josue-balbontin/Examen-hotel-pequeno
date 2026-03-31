import { Component, Output, EventEmitter, OnInit, ChangeDetectorRef, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ReservaApiService } from '../../../Services/api/reserva-api.service';
import { UsuarioApiService } from '../../../Services/api/usuario-api.service';
import { CrearReservaDto } from '../../../Models/crear-reserva.dto';
import { PantallaCargaComponent } from '../../pantallas_avisos/pantalla-carga/pantalla-carga.component';
import { MostrarerrorComponent } from '../../pantallas_avisos/mostrarerror/mostrarerror.component';
import { Usuario } from '../../../Models/usuario.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-crear-reserva',
  standalone: true,
  imports: [ReactiveFormsModule, PantallaCargaComponent, MostrarerrorComponent, NgClass],
  templateUrl: './crear-reserva.html',
  styleUrl: './crear-reserva.css',
})
export class CrearReserva implements OnInit {
  @Output() cerrar = new EventEmitter<void>();
  @Output() guardado = new EventEmitter<void>();

  reservaForm: FormGroup;
  cargando: WritableSignal<boolean> = signal(false);


  hayErrorFront: boolean = false;
  mensajeErrorFront: string = '';


  habitacionesDisponibles: any[] = [];
  tiposDisponibles: any[] = [];
  habitacionesFiltradas: any[] = [];
  usuariosDisponibles: Usuario[] = [];


  tipoSeleccionadoInfo: any = null;

  constructor(
    private fb: FormBuilder,
    private reservaApi: ReservaApiService,
    private usuarioApi: UsuarioApiService,
    private cdr: ChangeDetectorRef
  ) {
    this.reservaForm = this.fb.group({
      fechaIngreso: ['', Validators.required],
      fechaSalida: ['', Validators.required],
      idTipoHabitacion: [{ value: '', disabled: true }, Validators.required],
      idHabitacion: [{ value: '', disabled: true }, Validators.required],
      idsUsuarios: [[], Validators.required]
    });
  }

  ngOnInit(): void {
    this.cargarUsuarios();

    this.reservaForm.get('idTipoHabitacion')?.valueChanges.subscribe(idTipo => {
      this.reservaForm.get('idHabitacion')?.reset('');
      if (idTipo) {

        this.tipoSeleccionadoInfo = this.tiposDisponibles.find(t => t.idTipoHabitaciones === Number(idTipo));

        this.habitacionesFiltradas = this.habitacionesDisponibles.filter(h => h.idTipoHabitacion === Number(idTipo));
        this.reservaForm.get('idHabitacion')?.enable();
      } else {
        this.tipoSeleccionadoInfo = null;
        this.habitacionesFiltradas = [];
        this.reservaForm.get('idHabitacion')?.disable();
      }
    });


  }

  cargarUsuarios(): void {
    this.cargando.set(true);
    this.usuarioApi.obtenerUsuarios().subscribe({
      next: (data) => {
        this.usuariosDisponibles = data;
        this.cargando.set(false);
        this.cdr.detectChanges();
      },
      error: () => this.cargando.set(false)
    });
  }

  buscarDisponibilidad(): void {
    const fIng = this.reservaForm.get('fechaIngreso')?.value;
    const fSal = this.reservaForm.get('fechaSalida')?.value;

    if (!fIng || !fSal) {
      this.mostrarErrorFront("Debes ingresar ambas fechas para buscar habitaciones.");
      return;
    }

    if (new Date(fSal) < new Date(fIng)) {
      this.mostrarErrorFront("La fecha de salida no puede ser anterior a la de ingreso (HU-02).");
      return;
    }

    this.hayErrorFront = false;
    this.cargando.set(true);
    this.reservaApi.disponibilidadHabitaciones(fIng, fSal).subscribe({
      next: (data) => {
        this.habitacionesDisponibles = data;


        const tiposMap = new Map();
        data.forEach((hab: any) => {
          const t = hab.idTipoHabitacionNavigation;
          if (t && !tiposMap.has(t.idTipoHabitaciones)) {
            tiposMap.set(t.idTipoHabitaciones, t);
          }
        });

        this.tiposDisponibles = Array.from(tiposMap.values());

        if (this.tiposDisponibles.length === 0) {
          this.mostrarErrorFront("No hay habitaciones disponibles para estas fechas.");
          this.reservaForm.get('idTipoHabitacion')?.disable();
        } else {
          this.reservaForm.get('idTipoHabitacion')?.enable();
        }

        this.cargando.set(false);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.mostrarErrorFront("Error al buscar disponibilidad: " + (err.error?.message || err.message));
        this.cargando.set(false);
        this.cdr.detectChanges();
      }
    });
  }

  guardar(): void {
    if (this.reservaForm.invalid) {
      this.reservaForm.markAllAsTouched();
      return;
    }

    const val = this.reservaForm.value;


    if (this.tipoSeleccionadoInfo && val.idsUsuarios.length > this.tipoSeleccionadoInfo.capacidad) {
      this.mostrarErrorFront(`La cantidad de huéspedes seleccionados (${val.idsUsuarios.length}) supera la capacidad del Tipo de Habitación (${this.tipoSeleccionadoInfo.capacidad}).`);
      return;
    }

    this.hayErrorFront = false;
    this.cargando.set(true);

    const dto = new CrearReservaDto();

    dto.idsUsuarios = val.idsUsuarios.map((id: string | number) => Number(id));
    dto.idHabitacion = Number(val.idHabitacion);
    dto.fechaIngreso = val.fechaIngreso;
    dto.fechaSalida = val.fechaSalida;

    this.reservaApi.crearReserva(dto).subscribe({
      next: () => {
        this.cargando.set(false);
        this.guardado.emit();
      },
      error: (err) => {
        this.mostrarErrorFront("Error al crear reserva: " + (err.error?.message || err.message));
        this.cargando.set(false);
        this.cdr.detectChanges();
      }
    });
  }

  mostrarErrorFront(msg: string) {
    this.hayErrorFront = true;
    this.mensajeErrorFront = msg;
  }

  cancelar(): void {
    this.cerrar.emit();
  }
}
