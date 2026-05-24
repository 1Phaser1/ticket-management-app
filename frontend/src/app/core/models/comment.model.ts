export interface TicketComment {
  id: number;
  ticketId: number;
  text: string;
  createdAt: string;
}

export interface CommentCreate {
  text: string;
}
