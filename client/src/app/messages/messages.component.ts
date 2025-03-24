import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from '../_models/message';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [ButtonsModule, FormsModule, TimeagoModule, RouterModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  container = "unread";
  pageNumber = 1;
  pageSize = 10;

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessages(this.pageNumber, this.pageSize, this.container);
  }

  getRoute(message: Message) {
  // if (this.container === "outbox") return `/members/${message.recipientUsername}`;
  // else return `/members/${message.senderUsername}`;

  return this.container === "outbox"
    ? `/members/${message.recipientUsername}`
    : `/members/${message.senderUsername}`;
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

}
