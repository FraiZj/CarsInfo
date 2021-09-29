import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'users-filter',
  templateUrl: './users-filter.component.html',
  styleUrls: ['./users-filter.component.scss']
})
export class UsersFilterComponent {
  public search: string | null = '';

  constructor() { }

  public onSearch(): void {
    console.log(this.search);
  }
}
