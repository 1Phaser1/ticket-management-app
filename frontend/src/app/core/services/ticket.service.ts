import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentCreate, TicketComment } from '../models/comment.model';
import {
  AssignTicketRequest,
  TicketCreate,
  TicketDetail,
  TicketListItem,
  TicketUpdate,
  UpdateTicketStatusRequest
} from '../models/ticket.model';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5004/api/Tickets';

  getTickets(): Observable<TicketListItem[]> {
    return this.http.get<TicketListItem[]>(this.apiUrl);
  }

  getTicket(id: number): Observable<TicketDetail> {
    return this.http.get<TicketDetail>(`${this.apiUrl}/${id}`);
  }

  createTicket(ticket: TicketCreate): Observable<TicketDetail> {
    return this.http.post<TicketDetail>(this.apiUrl, ticket);
  }

  updateTicket(id: number, ticket: TicketUpdate): Observable<TicketDetail> {
    return this.http.put<TicketDetail>(`${this.apiUrl}/${id}`, ticket);
  }

  deleteTicket(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateStatus(id: number, request: UpdateTicketStatusRequest): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/status`, request);
  }

  assignTicket(id: number, request: AssignTicketRequest): Observable<TicketListItem> {
    return this.http.patch<TicketListItem>(`${this.apiUrl}/${id}/assign`, request);
  }

  addComment(ticketId: number, request: CommentCreate): Observable<TicketComment> {
    return this.http.post<TicketComment>(`${this.apiUrl}/${ticketId}/comments`, request);
  }
}
