import { DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { TICKET_STATUSES, TicketListItem } from '../../core/models/ticket.model';
import { User } from '../../core/models/user.model';
import { TicketService } from '../../core/services/ticket.service';
import { UserService } from '../../core/services/user.service';
import { VisitedPagesService } from '../../core/services/visited-pages.service';

@Component({
  selector: 'app-ticket-list',
  imports: [DatePipe, FormsModule, RouterLink],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.css'
})
export class TicketListComponent implements OnInit {
  private readonly ticketService = inject(TicketService);
  private readonly userService = inject(UserService);
  private readonly visitedPagesService = inject(VisitedPagesService);

  readonly statuses = TICKET_STATUSES;
  readonly tickets = signal<TicketListItem[]>([]);
  readonly users = signal<User[]>([]);
  readonly searchTerm = signal('');
  readonly statusFilter = signal('');
  readonly assignedToFilter = signal('');
  readonly loading = signal(true);
  readonly error = signal('');

  readonly filteredTickets = computed(() => {
    const searchValue = this.searchTerm().trim().toLowerCase();
    const statusValue = this.statusFilter();
    const assignedToValue = this.assignedToFilter();

    return this.tickets().filter((ticket) => {
      const matchesSearch =
        !searchValue ||
        ticket.title.toLowerCase().includes(searchValue) ||
        ticket.status.toLowerCase().includes(searchValue) ||
        ticket.priority.toLowerCase().includes(searchValue) ||
        (ticket.assignedToFullName ?? '').toLowerCase().includes(searchValue);

      const matchesStatus = !statusValue || ticket.status === statusValue;
      const matchesAssignedTo =
        !assignedToValue ||
        (assignedToValue === 'unassigned'
          ? ticket.assignedToId === null
          : ticket.assignedToId === Number(assignedToValue));

      return matchesSearch && matchesStatus && matchesAssignedTo;
    });
  });

  ngOnInit(): void {
    this.visitedPagesService.addPage('Ticket Listesi', '/tickets');
    this.loadData();
  }

  setSearchTerm(value: string): void {
    this.searchTerm.set(value);
  }

  setStatusFilter(value: string): void {
    this.statusFilter.set(value);
  }

  setAssignedToFilter(value: string): void {
    this.assignedToFilter.set(value);
  }

  badgeClass(value: string): string {
    return value.toLowerCase().replaceAll(' ', '-');
  }

  private loadData(): void {
    this.loading.set(true);
    this.error.set('');

    forkJoin({
      tickets: this.ticketService.getTickets(),
      users: this.userService.getUsers()
    }).subscribe({
      next: ({ tickets, users }) => {
        this.tickets.set(tickets);
        this.users.set(users);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Ticket verileri yuklenemedi. Backend API calisiyor mu kontrol edin.');
        this.loading.set(false);
      }
    });
  }
}
