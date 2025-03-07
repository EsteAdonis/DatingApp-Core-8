import { Component, input } from '@angular/core';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-memeber-card',
  standalone: true,
  imports: [],
  templateUrl: './memeber-card.component.html',
  styleUrl: './memeber-card.component.css',
})
export class MemeberCardComponent {
  member = input.required<Member>();
}
