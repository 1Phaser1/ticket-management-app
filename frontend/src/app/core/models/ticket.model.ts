import { TicketComment } from './comment.model';
import { User } from './user.model';

export interface TicketListItem {
  id: number;
  title: string;
  status: string;
  priority: string;
  assignedToId: number | null;
  assignedToFullName: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface TicketDetail {
  id: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  assignedToId: number | null;
  assignedTo: User | null;
  createdAt: string;
  updatedAt: string | null;
  comments: TicketComment[];
}

export interface TicketCreate {
  title: string;
  description: string;
  status: string;
  priority: string;
  assignedToId: number | null;
}

export type TicketUpdate = TicketCreate;

export interface AssignTicketRequest {
  assignedToId: number | null;
}

export interface UpdateTicketStatusRequest {
  status: string;
}

export const TICKET_STATUSES = ['Open', 'In Progress', 'Resolved', 'Closed'] as const;

export const TICKET_PRIORITIES = ['Low', 'Medium', 'High', 'Critical'] as const;
