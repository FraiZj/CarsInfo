import { fetchUsers } from './../../store/actions/users-list.actions';
import { selectUsers } from './../../store/selectors/users-list.selectors';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { User } from '@users-shared';

@Component({
  selector: 'users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements OnInit {
  public users$: Observable<User[]> = this.store.select(selectUsers);

  constructor(
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.store.dispatch(fetchUsers());
  }

  public mailTo(email: string) {
    return `mailto:${email}`;
  }
}
