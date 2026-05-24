import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { RecentlyVisitedComponent } from './shared/components/recently-visited/recently-visited.component';

@Component({
  selector: 'app-root',
  imports: [RecentlyVisitedComponent, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {}
