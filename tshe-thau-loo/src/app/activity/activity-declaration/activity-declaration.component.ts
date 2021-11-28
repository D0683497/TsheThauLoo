import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-activity-declaration',
  templateUrl: './activity-declaration.component.html',
  styleUrls: ['./activity-declaration.component.scss'],
})
export class ActivityDeclarationComponent implements OnInit {

  @Input() declaration: string;
  date = Date.now();

  constructor(private modalController: ModalController) { }

  ngOnInit(): void {}

  async dismiss(agree: boolean): Promise<void> {
    await this.modalController.dismiss(agree);
  }

}
