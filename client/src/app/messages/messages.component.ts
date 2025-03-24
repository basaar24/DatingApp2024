import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages.service';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  container = "outbox";
  pageNumber = 1;
  pageSize = 10;

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessages(this.pageNumber, this.pageSize, this.container);
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

}
