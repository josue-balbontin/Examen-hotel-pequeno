import { Component, OnInit, ChangeDetectorRef, WritableSignal, signal } from '@angular/core';
import { TablaGenerica } from '../tabla-generica/tabla-generica';
import { ColumnaTabla, AccionTabla } from '../../Models/tabla-generica/tabla-generica';
import { UsuarioApiService } from '../../Services/api/usuario-api.service';
import { Usuario } from '../../Models/usuario.model';
import { RegistrarUsuario } from './registrar-usuario/registrar-usuario';

@Component({
  selector: 'app-usuarios',
  imports: [TablaGenerica, RegistrarUsuario],
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

  constructor(private usuarioApi: UsuarioApiService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios(): void {
    this.usuarioApi.obtenerUsuarios().subscribe({
      next: (data) => {
        this.usuarios = [...data];
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al cargar usuarios:', err);
      }
    });
  }

  manejarAccion(event: { nombre: string; item: any }): void {
    switch (event.nombre) {
      case 'editar':
        console.log('Editar usuario:', event.item);
        break;
      case 'eliminar':
        console.log('Eliminar usuario:', event.item);
        break;
    }
  }

  agregarUsuario(): void {
    this.registrarUsuario.set(true);
  }





}
