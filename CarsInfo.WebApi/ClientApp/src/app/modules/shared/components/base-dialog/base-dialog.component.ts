import { Component, OnInit, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'base-dialog',
  templateUrl: './base-dialog.component.html',
  styleUrls: ['./base-dialog.component.scss']
})
export class BaseDialogComponent {
  @Input() title!: string;

  constructor(
    public readonly dialogRef: MatDialogRef<BaseDialogComponent>
  ) { }

  public closeDialog(): void {
    this.dialogRef.close();
  }
}
