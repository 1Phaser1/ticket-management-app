import { Injectable, signal } from '@angular/core';

export interface VisitedPage {
  title: string;
  url: string;
  visitedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class VisitedPagesService {
  private readonly storageKey = 'ticket-management-recent-pages';
  private readonly pagesSignal = signal<VisitedPage[]>(this.readPages());

  readonly pages = this.pagesSignal.asReadonly();

  addPage(title: string, url: string): void {
    const normalizedTitle = title.trim();
    const normalizedUrl = url.trim();

    if (!normalizedTitle || !normalizedUrl) {
      return;
    }

    const page: VisitedPage = {
      title: normalizedTitle,
      url: normalizedUrl,
      visitedAt: new Date().toISOString()
    };

    const nextPages = [
      page,
      ...this.pagesSignal().filter((item) => item.url !== normalizedUrl)
    ].slice(0, 6);

    this.pagesSignal.set(nextPages);
    this.writePages(nextPages);
  }

  private readPages(): VisitedPage[] {
    if (!this.canUseLocalStorage()) {
      return [];
    }

    const storedValue = localStorage.getItem(this.storageKey);

    if (!storedValue) {
      return [];
    }

    try {
      const parsedValue = JSON.parse(storedValue) as VisitedPage[];
      return Array.isArray(parsedValue) ? parsedValue : [];
    } catch {
      return [];
    }
  }

  private writePages(pages: VisitedPage[]): void {
    if (!this.canUseLocalStorage()) {
      return;
    }

    localStorage.setItem(this.storageKey, JSON.stringify(pages));
  }

  private canUseLocalStorage(): boolean {
    return typeof localStorage !== 'undefined';
  }
}
