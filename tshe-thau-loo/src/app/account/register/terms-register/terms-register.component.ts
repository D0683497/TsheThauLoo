import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-terms-register',
  templateUrl: './terms-register.component.html',
  styleUrls: ['./terms-register.component.scss'],
})
export class TermsRegisterComponent implements OnInit {

  date = Date.now();

  constructor(private modalController: ModalController) { }

  ngOnInit(): void {}

  async dismiss(agree: boolean): Promise<void> {
    await this.modalController.dismiss(agree);
  }

}
