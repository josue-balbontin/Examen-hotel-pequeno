import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SideBard } from './Components/side-bard/side-bard';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SideBard],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}
