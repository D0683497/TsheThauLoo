import { Injectable } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { TermsRegisterComponent } from '../../account/register/terms-register/terms-register.component';
import { IAdministratorInfo } from '../../models/account/profile/administrator/administrator-info.model';
import { IResponsibility } from '../../models/account/profile/administrator/responsibility.model';
// eslint-disable-next-line max-len
import { AdministratorCreateResponsibilityComponent } from '../../account/profile/administrator/administrator-create-responsibility/administrator-create-responsibility.component';
// eslint-disable-next-line max-len
import { AdministratorEditResponsibilityComponent } from '../../account/profile/administrator/administrator-edit-responsibility/administrator-edit-responsibility.component';
import { IDocument } from '../../models/document/document.model';
import { StudentEditVerifyFileComponent } from '../../account/profile/student/student-edit-verify-file/student-edit-verify-file.component';

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
    const { data } = await this.modal.onDidDismiss<boolean>();
    return data;
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

  async editStudentVerifyFile(file: IDocument): Promise<IDocument> {
    this.modal = await this.modalController.create({
      component: StudentEditVerifyFileComponent,
      componentProps: {file},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IDocument>();
    return data;
  }

}
