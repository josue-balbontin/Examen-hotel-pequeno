import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ColumnaTabla, AccionTabla } from '../../Models/tabla-generica/tabla-generica';

@Component({
  selector: 'app-tabla-generica',
  imports: [],
  templateUrl: './tabla-generica.html',
  styleUrl: './tabla-generica.css',
})
export class TablaGenerica {
  @Input() columnas: ColumnaTabla[] = [];
  @Input() datos: any[] = [];
  @Input() acciones: AccionTabla[] = [];

  @Output() accion = new EventEmitter<{ nombre: string; item: any }>();

  getValor(item: any, field: string): any {
    return item[field] ?? '-';
  }

  ejecutarAccion(nombre: string, item: any): void {
    this.accion.emit({ nombre, item });
  }
}
