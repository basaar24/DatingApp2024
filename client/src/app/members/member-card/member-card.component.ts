import { Component, computed, inject, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  encapsulation: ViewEncapsulation.None
})
export class MemberCardComponent {
  private likesService = inject(LikesService);
  member = input.required<Member>();
  hasLiked = computed(() => this.likesService.likeIds().includes(this.member().id));
}
