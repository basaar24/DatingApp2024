import { Component, inject, input, OnInit } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { Message } from '../../_models/message';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit {
  private messagesService = inject(MessagesService);
  username = input.required<string>();
  messages = input.required<Message[]>();

  ngOnInit(): void {
  }
}
