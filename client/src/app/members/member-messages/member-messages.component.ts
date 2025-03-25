import { Component, inject, input, OnInit } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { Message } from '../../_models/message';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit {
  private messagesService = inject(MessagesService);
  username = input.required<string>();
  messages: Message[] = [];

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessageThread(this.username()).subscribe({
      next: messages => this.messages = messages
    });
  }
}
