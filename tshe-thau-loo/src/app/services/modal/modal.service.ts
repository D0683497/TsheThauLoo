import { Injectable } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { TermsRegisterComponent } from '../../account/register/terms-register/terms-register.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  modal: HTMLIonModalElement;

  constructor(private modalController: ModalController) { }

  async registerTerms(): Promise<boolean> {
    this.modal = await this.modalController.create({
      component: TermsRegisterComponent,
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss();
    return data.agree;
  }

}
