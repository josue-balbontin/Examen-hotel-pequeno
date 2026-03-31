import { Component, OnInit, ChangeDetectorRef, WritableSignal, signal } from '@angular/core';
import { TablaGenerica } from '../tabla-generica/tabla-generica';
import { ColumnaTabla, AccionTabla } from '../../Models/tabla-generica/tabla-generica';
import { UsuarioApiService } from '../../Services/api/usuario-api.service';
import { Usuario } from '../../Models/usuario.model';
import { RegistrarUsuario } from './registrar-usuario/registrar-usuario';
import { PantallaCargaComponent } from '../pantallas_avisos/pantalla-carga/pantalla-carga.component';

@Component({
  selector: 'app-usuarios',
  imports: [TablaGenerica, RegistrarUsuario, PantallaCargaComponent],
  templateUrl: './usuarios.html',
  styleUrl: './usuarios.css',
})
export class Usuarios implements OnInit {

  usuarios: Usuario[] = [];

  columnas: ColumnaTabla[] = [
    { titulo: 'ID', contenido: 'idUsuarios' },
    { titulo: 'Nombres', contenido: 'nombres' },
    { titulo: 'Apellidos', contenido: 'apellidos' },
    { titulo: 'Documento', contenido: 'documentoIdentidad' },
    { titulo: 'Teléfono', contenido: 'telefono' },
    { titulo: 'Email', contenido: 'email' },
    { titulo: 'Edad', contenido: 'edad' },
  ];

  acciones: AccionTabla[] = [

  ];


  registrarUsuario: WritableSignal<boolean> = signal<boolean>(false);
  cargando: WritableSignal<boolean> = signal<boolean>(false);

  constructor(private usuarioApi: UsuarioApiService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios(): void {
    this.cargando.set(true);
    this.usuarioApi.obtenerUsuarios().subscribe({
      next: (data) => {
        this.usuarios = [...data];
        this.cargando.set(false);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al cargar usuarios:', err);
        this.cargando.set(false);
        this.cdr.detectChanges();
      }
    });
  }



  agregarUsuario(): void {
    this.registrarUsuario.set(true);
  }

  cerrarModalRegistro(): void {
    this.registrarUsuario.set(false);
  }

  onHuespedGuardado(): void {
    this.cerrarModalRegistro();
    this.cargarUsuarios();
  }

}
