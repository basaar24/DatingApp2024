import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from '../_models/message';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [ButtonsModule, FormsModule, TimeagoModule, RouterModule, PaginationModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  container = "Inbox";
  pageNumber = 1;
  pageSize = 5;
  isOutbox = this.container === "Outbox";

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessages(this.pageNumber, this.pageSize, this.container.toLocaleLowerCase());
  }

  deleteMessage(id: number) {
    this.messagesService.deleteMessage(id).subscribe({
      next: _ => {
        this.messagesService.paginatedResult.update(prev => {
          if (prev && prev.items) {
            prev.items.splice(prev.items.findIndex(m => m.id == id), 1);
            return prev;
          }
          return prev;
        });
      }
    });
  }

  getRoute(message: Message) {
    if (this.isOutbox) {
      return `/members/${message.recipientUsername}`;
    }
    else {
      return `/members/${message.senderUsername}`;
    }
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

}
