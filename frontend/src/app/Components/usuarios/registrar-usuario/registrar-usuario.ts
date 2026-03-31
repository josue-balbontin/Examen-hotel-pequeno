import { Component, EventEmitter, Output, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UsuarioApiService } from '../../../Services/api/usuario-api.service';
import { RegistrarUsuarioDto } from '../../../Models/registrar-usuario.dto';
import { MostrarerrorComponent } from '../../pantallas_avisos/mostrarerror/mostrarerror.component';

@Component({
  selector: 'app-registrar-usuario',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MostrarerrorComponent],
  templateUrl: './registrar-usuario.html',
  styleUrl: './registrar-usuario.css',
})
export class RegistrarUsuario {
  @Output() cerrar = new EventEmitter<void>();
  @Output() guardado = new EventEmitter<void>();

  usuarioForm: FormGroup;


  error: WritableSignal<boolean> = signal(false);
  mensajeError: string = '';

  estaGuardando: boolean = false;

  constructor(private fb: FormBuilder, private usuarioApi: UsuarioApiService) {
    this.usuarioForm = this.fb.group({
      nombres: ['', [Validators.required, Validators.maxLength(100)]],
      apellidos: ['', [Validators.required, Validators.maxLength(100)]],
      documentoIdentidad: ['', [Validators.required, Validators.maxLength(20)]],
      telefono: ['', [Validators.maxLength(20)]],
      email: ['', [Validators.email, Validators.maxLength(150)]],
      edad: [null, [Validators.min(0), Validators.max(150)]]
    });
  }

  cerrarModal() {
    this.cerrar.emit();
  }

  guardar() {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      return;
    }

    this.estaGuardando = true;
    const payload: RegistrarUsuarioDto = this.usuarioForm.value;

    this.usuarioApi.registrarUsuario(payload).subscribe({
      next: () => {
        this.estaGuardando = false;
        this.guardado.emit();
      },
      error: (err) => {
        this.estaGuardando = false;

        if (err.error && err.error.error) {
          this.mensajeError = err.error.error;
        }
        else {
          this.mensajeError = "Ha ocurrido un error inesperado al guardar el Usuario.";
        }

        this.error.set(true);
      }
    });
  }
}
