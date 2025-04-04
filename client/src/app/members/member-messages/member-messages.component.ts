import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { Message } from '../../_models/message';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent {
  @ViewChild("messageForm") messageForm?: NgForm;
  private messagesService = inject(MessagesService);
  username = input.required<string>();
  messages = input.required<Message[]>();
  messageContent = "";
  updateMessages = output<Message>();

  sendMessage() {
    this.messagesService.sendMessage(this.username(), this.messageContent).subscribe({
      next: message => {
        this.updateMessages.emit(message);
        this.messageForm?.reset();
      }
    });
  }
}
