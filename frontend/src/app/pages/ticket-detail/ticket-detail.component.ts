import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { TICKET_STATUSES, TicketDetail } from '../../core/models/ticket.model';
import { User } from '../../core/models/user.model';
import { TicketService } from '../../core/services/ticket.service';
import { UserService } from '../../core/services/user.service';
import { VisitedPagesService } from '../../core/services/visited-pages.service';

@Component({
  selector: 'app-ticket-detail',
  imports: [DatePipe, FormsModule, RouterLink],
  templateUrl: './ticket-detail.component.html',
  styleUrl: './ticket-detail.component.css'
})
export class TicketDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly ticketService = inject(TicketService);
  private readonly userService = inject(UserService);
  private readonly visitedPagesService = inject(VisitedPagesService);

  readonly statuses = TICKET_STATUSES;
  readonly ticket = signal<TicketDetail | null>(null);
  readonly users = signal<User[]>([]);
  readonly selectedStatus = signal('');
  readonly selectedAssignedToId = signal('');
  readonly commentText = signal('');
  readonly loading = signal(true);
  readonly savingStatus = signal(false);
  readonly savingAssignment = signal(false);
  readonly savingComment = signal(false);
  readonly error = signal('');

  private ticketId = 0;

  ngOnInit(): void {
    this.ticketId = Number(this.route.snapshot.paramMap.get('id'));

    if (!Number.isInteger(this.ticketId) || this.ticketId <= 0) {
      this.error.set('Gecersiz ticket id.');
      this.loading.set(false);
      return;
    }

    this.loadData();
  }

  setStatus(value: string): void {
    this.selectedStatus.set(value);
  }

  setAssignedTo(value: string): void {
    this.selectedAssignedToId.set(value);
  }

  setCommentText(value: string): void {
    this.commentText.set(value);
  }

  saveStatus(): void {
    const currentTicket = this.ticket();
    const status = this.selectedStatus();

    if (!currentTicket || !status || status === currentTicket.status) {
      return;
    }

    this.savingStatus.set(true);
    this.ticketService.updateStatus(currentTicket.id, { status }).subscribe({
      next: () => {
        this.ticket.set({
          ...currentTicket,
          status,
          updatedAt: new Date().toISOString()
        });
        this.savingStatus.set(false);
      },
      error: () => {
        this.error.set('Status guncellenemedi.');
        this.savingStatus.set(false);
      }
    });
  }

  saveAssignment(): void {
    const currentTicket = this.ticket();

    if (!currentTicket) {
      return;
    }

    const assignedToId = this.selectedAssignedToId()
      ? Number(this.selectedAssignedToId())
      : null;

    if (assignedToId === currentTicket.assignedToId) {
      return;
    }

    this.savingAssignment.set(true);
    this.ticketService.assignTicket(currentTicket.id, { assignedToId }).subscribe({
      next: () => {
        const assignedTo = this.users().find((user) => user.id === assignedToId) ?? null;
        this.ticket.set({
          ...currentTicket,
          assignedToId,
          assignedTo,
          updatedAt: new Date().toISOString()
        });
        this.savingAssignment.set(false);
      },
      error: () => {
        this.error.set('Atama guncellenemedi.');
        this.savingAssignment.set(false);
      }
    });
  }

  addComment(): void {
    const currentTicket = this.ticket();
    const text = this.commentText().trim();

    if (!currentTicket || !text) {
      return;
    }

    this.savingComment.set(true);
    this.ticketService.addComment(currentTicket.id, { text }).subscribe({
      next: (comment) => {
        this.ticket.set({
          ...currentTicket,
          updatedAt: new Date().toISOString(),
          comments: [...currentTicket.comments, comment]
        });
        this.commentText.set('');
        this.savingComment.set(false);
      },
      error: () => {
        this.error.set('Yorum eklenemedi.');
        this.savingComment.set(false);
      }
    });
  }

  badgeClass(value: string): string {
    return value.toLowerCase().replaceAll(' ', '-');
  }

  private loadData(): void {
    this.loading.set(true);
    this.error.set('');

    forkJoin({
      ticket: this.ticketService.getTicket(this.ticketId),
      users: this.userService.getUsers()
    }).subscribe({
      next: ({ ticket, users }) => {
        this.ticket.set(ticket);
        this.users.set(users);
        this.selectedStatus.set(ticket.status);
        this.selectedAssignedToId.set(ticket.assignedToId?.toString() ?? '');
        this.visitedPagesService.addPage(`#${ticket.id} ${ticket.title}`, `/tickets/${ticket.id}`);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Ticket detayi yuklenemedi.');
        this.loading.set(false);
      }
    });
  }
}
