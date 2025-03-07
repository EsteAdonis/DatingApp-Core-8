import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemeberCardComponent } from '../memeber-card/memeber-card.component';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemeberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private memberService = inject(MembersService);
  members: Member[] = [];

  ngOnInit(): void {
   this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMemebers().subscribe({
      next: members => this.members = members
    })
  }

}
