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
import { EditVerifyFileComponent } from '../../account/profile/edit-verify-file/edit-verify-file.component';
import { RoleType } from '../../enums/role-type.enum';
// eslint-disable-next-line max-len
import { IndustrialClassificationCreateComponent } from '../../company/industrial-classification-create/industrial-classification-create.component';
import { ICompany } from '../../models/company/company.model';
import { IIndustrialClassification } from '../../models/company/industrial-classification.model';
// eslint-disable-next-line max-len
import { IndustrialClassificationEditComponent } from '../../company/industrial-classification-edit/industrial-classification-edit.component';
import { ResumeFileEditComponent } from '../../resume/resume-file-edit/resume-file-edit.component';
import { ActivityFileEditComponent } from '../../activity/activity-file-edit/activity-file-edit.component';
import { ActivityType } from '../../enums/activity-type.enum';
import { ActivityDeclarationComponent } from '../../activity/activity-declaration/activity-declaration.component';
import { ActivitySignInComponent } from '../../activity/activity-sign-in/activity-sign-in.component';
import { ActivityQrCodeComponent } from '../../activity/activity-qr-code/activity-qr-code.component';
import { ActivityInviteCompanyComponent } from '../../activity/activity-invite-company/activity-invite-company.component';
import { IFaculty } from '../../models/opening/faculty.model';
import { FacultyCreateComponent } from '../../activity/faculty/faculty-create/faculty-create.component';
import { QualificationCreateComponent } from '../../activity/qualification/qualification-create/qualification-create.component';
import { FacultyEditComponent } from '../../activity/faculty/faculty-edit/faculty-edit.component';
import { IQualification } from '../../models/opening/qualification.model';
import { QualificationEditComponent } from '../../activity/qualification/qualification-edit/qualification-edit.component';
import { ResumeSelectComponent } from '../../activity/resume-select/resume-select.component';
import { DeliveryResumeListComponent } from '../../activity/delivery-resume-list/delivery-resume-list.component';

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

  async editVerifyFile(role: RoleType, file: IDocument): Promise<IDocument> {
    this.modal = await this.modalController.create({
      component: EditVerifyFileComponent,
      componentProps: {role, file},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IDocument>();
    return data;
  }

  async createSIC(companyId: string): Promise<ICompany> {
    this.modal = await this.modalController.create({
      component: IndustrialClassificationCreateComponent,
      componentProps: {companyId},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<ICompany>();
    return data;
  }

  async editSIC(companyId: string, industrialClassification: IIndustrialClassification): Promise<ICompany> {
    this.modal = await this.modalController.create({
      component: IndustrialClassificationEditComponent,
      componentProps: {companyId, industrialClassification},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<ICompany>();
    return data;
  }

  async editFileResume(resume: IDocument): Promise<IDocument> {
    this.modal = await this.modalController.create({
      component: ResumeFileEditComponent,
      componentProps: {resume},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IDocument>();
    return data;
  }

  async editActivityFile(type: ActivityType, firstId: string, secondId: string, file: IDocument): Promise<IDocument> {
    this.modal = await this.modalController.create({
      component: ActivityFileEditComponent,
      componentProps: {type, firstId, secondId, file},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IDocument>();
    return data;
  }

  async activityDeclaration(declaration: string): Promise<boolean> {
    this.modal = await this.modalController.create({
      component: ActivityDeclarationComponent,
      componentProps: {declaration},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<boolean>();
    return data;
  }

  async activityQRCode(title: string, uri: string): Promise<boolean> {
    this.modal = await this.modalController.create({
      component: ActivityQrCodeComponent,
      componentProps: {title, uri},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<boolean>();
    return data;
  }

  async activitySignIn(firstId: string, secondId: string, type: ActivityType): Promise<void> {
    this.modal = await this.modalController.create({
      component: ActivitySignInComponent,
      componentProps: {firstId, secondId, type},
      swipeToClose: true
    });
    await this.modal.present();
  }

  async activityInviteCompany(): Promise<string> {
    this.modal = await this.modalController.create({
      component: ActivityInviteCompanyComponent,
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<string>();
    return data;
  }

  async createFaculty(campaignId: string, recruitmentId: string, openingId: string): Promise<IFaculty> {
    this.modal = await this.modalController.create({
      component: FacultyCreateComponent,
      componentProps: {campaignId, recruitmentId, openingId},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IFaculty>();
    return data;
  }

  async createQualification(campaignId: string, recruitmentId: string, openingId: string): Promise<IQualification> {
    this.modal = await this.modalController.create({
      component: QualificationCreateComponent,
      componentProps: {campaignId, recruitmentId, openingId},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IQualification>();
    return data;
  }

  async editFaculty(campaignId: string, recruitmentId: string, openingId: string, faculty: IFaculty): Promise<IFaculty> {
    this.modal = await this.modalController.create({
      component: FacultyEditComponent,
      componentProps: {campaignId, recruitmentId, openingId, faculty},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IFaculty>();
    return data;
  }

  async editQualification(campaignId: string, recruitmentId: string, openingId: string, qualification: IQualification): Promise<IFaculty> {
    this.modal = await this.modalController.create({
      component: QualificationEditComponent,
      componentProps: {campaignId, recruitmentId, openingId, qualification},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<IQualification>();
    return data;
  }

  async deliveryResume(campaignId: string, recruitmentId: string, openingId: string): Promise<string> {
    this.modal = await this.modalController.create({
      component: ResumeSelectComponent,
      componentProps: {campaignId, recruitmentId, openingId},
      swipeToClose: true
    });
    await this.modal.present();
    const { data } = await this.modal.onDidDismiss<string>();
    return data;
  }

  async showDeliveryResume(campaignId: string, recruitmentId: string, openingId: string): Promise<void> {
    this.modal = await this.modalController.create({
      component: DeliveryResumeListComponent,
      componentProps: {campaignId, recruitmentId, openingId},
      swipeToClose: true
    });
    await this.modal.present();
  }

}
