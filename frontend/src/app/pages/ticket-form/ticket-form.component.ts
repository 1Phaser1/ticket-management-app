import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import {
  TICKET_PRIORITIES,
  TICKET_STATUSES,
  TicketCreate
} from '../../core/models/ticket.model';
import { User } from '../../core/models/user.model';
import { TicketService } from '../../core/services/ticket.service';
import { UserService } from '../../core/services/user.service';
import { VisitedPagesService } from '../../core/services/visited-pages.service';

@Component({
  selector: 'app-ticket-form',
  imports: [FormsModule, RouterLink],
  templateUrl: './ticket-form.component.html',
  styleUrl: './ticket-form.component.css'
})
export class TicketFormComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly ticketService = inject(TicketService);
  private readonly userService = inject(UserService);
  private readonly visitedPagesService = inject(VisitedPagesService);

  readonly statuses = TICKET_STATUSES;
  readonly priorities = TICKET_PRIORITIES;
  readonly users = signal<User[]>([]);
  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEditMode = signal(false);
  readonly error = signal('');

  form: TicketCreate = {
    title: '',
    description: '',
    priority: 'Medium',
    status: 'Open',
    assignedToId: null
  };

  private ticketId: number | null = null;

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.ticketId = idParam ? Number(idParam) : null;
    this.isEditMode.set(this.ticketId !== null);

    if (this.ticketId !== null && (!Number.isInteger(this.ticketId) || this.ticketId <= 0)) {
      this.error.set('Gecersiz ticket id.');
      return;
    }

    this.loadData();
  }

  save(ticketForm: NgForm): void {
    if (ticketForm.invalid || this.saving()) {
      return;
    }

    const payload: TicketCreate = {
      title: this.form.title.trim(),
      description: this.form.description.trim(),
      priority: this.form.priority,
      status: this.form.status,
      assignedToId: this.form.assignedToId
    };

    this.saving.set(true);
    this.error.set('');

    const request$ =
      this.isEditMode() && this.ticketId !== null
        ? this.ticketService.updateTicket(this.ticketId, payload)
        : this.ticketService.createTicket(payload);

    request$.subscribe({
      next: (ticket) => {
        void this.router.navigate(['/tickets', ticket.id]);
      },
      error: () => {
        this.error.set('Ticket kaydedilemedi. Form alanlarini ve API baglantisini kontrol edin.');
        this.saving.set(false);
      }
    });
  }

  private loadData(): void {
    this.loading.set(true);
    this.error.set('');

    const ticketRequest =
      this.isEditMode() && this.ticketId !== null
        ? this.ticketService.getTicket(this.ticketId)
        : of(null);

    forkJoin({
      ticket: ticketRequest,
      users: this.userService.getUsers()
    }).subscribe({
      next: ({ ticket, users }) => {
        this.users.set(users);

        if (ticket) {
          this.form = {
            title: ticket.title,
            description: ticket.description,
            priority: ticket.priority,
            status: ticket.status,
            assignedToId: ticket.assignedToId
          };
          this.visitedPagesService.addPage(`#${ticket.id} Duzenle`, `/tickets/${ticket.id}/edit`);
        } else {
          this.visitedPagesService.addPage('Yeni Ticket', '/tickets/new');
        }

        this.loading.set(false);
      },
      error: () => {
        this.error.set('Form verileri yuklenemedi.');
        this.loading.set(false);
      }
    });
  }
}
