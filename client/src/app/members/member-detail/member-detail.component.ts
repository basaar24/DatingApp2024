import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessagesService } from '../../_services/messages.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit{
  @ViewChild("memberTabs", { static: true }) memberTabs?: TabsetComponent;
  private memberService = inject(MembersService);
  private messagesService = inject(MessagesService);
  private route = inject(ActivatedRoute);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

  ngOnInit(): void {
    // this.loadMember();

    this.route.data.subscribe({
      next: data => {
        this.member = data["member"];
        this.member && this.member.photos.map((photo) => {
          this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
        });
      }
    });

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    });
  }

  onUpdateMessages(event: Message) {
    this.messages.push(event);
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const messageTab = this.memberTabs.tabs.find(t => t.heading === heading);
      if (messageTab) messageTab.active = true;
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === "Messages" && this.messages.length === 0 && this.member) {
      this.messagesService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      });
    }
  }

  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get("username");
  //   console.log("Username:" + username);

  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: (member) => {
  //       this.member = member;
  //       member.photos.map((photo) => {
  //         this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
  //       });
  //     }
  //   });
  // }
}
