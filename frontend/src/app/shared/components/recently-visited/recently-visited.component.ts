import { DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { VisitedPagesService } from '../../../core/services/visited-pages.service';

@Component({
  selector: 'app-recently-visited',
  imports: [DatePipe, RouterLink],
  templateUrl: './recently-visited.component.html',
  styleUrl: './recently-visited.component.css'
})
export class RecentlyVisitedComponent {
  private readonly visitedPagesService = inject(VisitedPagesService);

  readonly pages = this.visitedPagesService.pages;
}
