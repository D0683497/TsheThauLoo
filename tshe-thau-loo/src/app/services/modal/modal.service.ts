import { Injectable } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { TermsRegisterComponent } from '../../account/register/terms-register/terms-register.component';
import { IAdministratorInfo } from '../../models/account/profile/administrator/administrator-info.model';
import { IResponsibility } from '../../models/account/profile/administrator/responsibility.model';
// eslint-disable-next-line max-len
import { AdministratorCreateResponsibilityComponent } from '../../account/profile/administrator/administrator-create-responsibility/administrator-create-responsibility.component';
// eslint-disable-next-line max-len
import { AdministratorEditResponsibilityComponent } from '../../account/profile/administrator/administrator-edit-responsibility/administrator-edit-responsibility.component';

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

  async createAdministratorResponsibility(): Promise<IAdministratorInfo> {
    this.modal = await this.modalController.create({
      component: AdministratorCreateResponsibilityComponent,
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IAdministratorInfo>();
    return data;
  }

  async editAdministratorResponsibility(responsibility: IResponsibility): Promise<IAdministratorInfo> {
    this.modal = await this.modalController.create({
      component: AdministratorEditResponsibilityComponent,
      componentProps: {responsibility},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IAdministratorInfo>();
    return data;
  }

}
