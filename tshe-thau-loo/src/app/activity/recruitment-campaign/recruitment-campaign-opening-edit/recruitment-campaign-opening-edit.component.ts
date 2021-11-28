import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../../services/loading/loading.service';
import { ActionSheetController, AlertController } from '@ionic/angular';
import { ModalService } from '../../../services/modal/modal.service';
import { OpeningService } from '../../../services/opening/opening.service';
import { HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { IOpening } from '../../../models/opening/opening.model';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IOpeningEdit } from '../../../models/opening/opening-edit.model';
import { IFaculty } from '../../../models/opening/faculty.model';
import { IQualification } from '../../../models/opening/qualification.model';

@Component({
  selector: 'app-recruitment-campaign-opening-edit',
  templateUrl: './recruitment-campaign-opening-edit.component.html',
  styleUrls: ['./recruitment-campaign-opening-edit.component.scss'],
})
export class RecruitmentCampaignOpeningEditComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  recruitmentId = this.route.snapshot.paramMap.get('recruitmentId');
  openingId = this.route.snapshot.paramMap.get('openingId');
  opening: IOpening;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  segment = 'opening';

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private openingService: OpeningService,
    private route: ActivatedRoute,
    private actionSheetController: ActionSheetController,
    private modalService: ModalService,
    private alertController: AlertController) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.openingService.getOpening(this.campaignId, this.recruitmentId, this.openingId).subscribe(
      (res: IOpening) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IOpening): Promise<void> {
    this.opening = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IOpening): void {
    this.editForm = this.fb.group({
      divisionName: [data.divisionName, [Validators.required, Validators.maxLength(30)]],
      jobTitle: [data.jobTitle, [Validators.required, Validators.maxLength(30)]],
      jobDescription: [data.jobDescription, [Validators.required]],
      workPlace: [data.workPlace, [Validators.required, Validators.maxLength(200)]],
      salary: [data.salary, [Validators.required, Validators.maxLength(20)]],
      requiredNumber: [data.requiredNumber, [Validators.required, Validators.min(0)]],
      education: [data.education, [Validators.required]],
      workExperience: [data.workExperience, [Validators.required, Validators.maxLength(500)]],
      language: [data.language, [Validators.required, Validators.maxLength(100)]],
      nationality: [data.nationality, [Validators.required, Validators.maxLength(20)]],
      isAccessibility: [data.isAccessibility, [Validators.required]]
    });
  }

  async onSubmit(data: IOpeningEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.openingService.editOpening(this.campaignId, this.recruitmentId, this.openingId, data).subscribe(
      (res: IOpening) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IOpening): Promise<void> {
    await this.loadingService.end();
    this.opening = res;
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('修改失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const errors: IServerError = err.error;
        await this.router.navigate(['/act']);
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
      }
    }
  }

  async optionFaculty(data: IFaculty): Promise<void> {
    const options = {
      translucent: true,
      header: '需求科系',
      buttons: [
        {
          text: '編輯',
          handler: () => {
            this.editFaculty(data);
          }
        },
        {
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.deleteFaculty(data);
          }
        },
        {
          text: '取消',
          icon: 'close',
          role: 'cancel',
        }
      ]
    };
    const actionSheet = await this.actionSheetController.create(options);
    await actionSheet.present();
  }

  async createFaculty(): Promise<void> {
    const res = await this.modalService.createFaculty(this.campaignId, this.recruitmentId, this.openingId);
    if (res !== undefined) {
      this.opening.faculties.push(res);
    }
  }

  async editFaculty(data: IFaculty): Promise<void> {
    const res = await this.modalService.editFaculty(this.campaignId, this.recruitmentId, this.openingId, data);
    if (res !== undefined) {
      const index = this.opening.faculties.findIndex(x => x.id === res.id);
      this.opening.faculties[index] = res;
    }
  }

  async deleteFaculty(data: IFaculty): Promise<void> {
    await this.loadingService.start('刪除中...');
    this.openingService.deleteFaculty(this.campaignId, this.recruitmentId, this.openingId, data.id).subscribe(
      () => { this.deleteFacultySuccess(data.id); },
      (err: HttpErrorResponse) => { this.deleteFacultyFail(err); }
    );
  }

  async deleteFacultySuccess(facultyId: string): Promise<void> {
    const index = this.opening.faculties.findIndex(x => x.id === facultyId);
    this.opening.faculties.splice(index, 1);
    await this.loadingService.end();
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteFacultyFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('刪除失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async optionQualification(data: IQualification): Promise<void> {
    const options = {
      translucent: true,
      header: '資格條件',
      buttons: [
        {
          text: '編輯',
          handler: () => {
            this.editQualification(data);
          }
        },
        {
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.deleteQualification(data);
          }
        },
        {
          text: '取消',
          icon: 'close',
          role: 'cancel',
        }
      ]
    };
    const actionSheet = await this.actionSheetController.create(options);
    await actionSheet.present();
  }

  async createQualification(): Promise<void> {
    const res = await this.modalService.createQualification(this.campaignId, this.recruitmentId, this.openingId);
    if (res !== undefined) {
      this.opening.qualifications.push(res);
    }
  }

  async editQualification(data: IQualification): Promise<void> {
    const res = await this.modalService.editQualification(this.campaignId, this.recruitmentId, this.openingId, data);
    if (res !== undefined) {
      const index = this.opening.qualifications.findIndex(x => x.id === res.id);
      this.opening.qualifications[index] = res;
    }
  }

  async deleteQualification(data: IQualification): Promise<void> {
    await this.loadingService.start('刪除中...');
    this.openingService.deleteQualification(this.campaignId, this.recruitmentId, this.openingId, data.id).subscribe(
      () => { this.deleteQualificationSuccess(data.id); },
      (err: HttpErrorResponse) => { this.deleteQualificationFail(err); }
    );
  }

  async deleteQualificationSuccess(qualificationId: string): Promise<void> {
    const index = this.opening.qualifications.findIndex(x => x.id === qualificationId);
    this.opening.qualifications.splice(index, 1);
    await this.loadingService.end();
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteQualificationFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('刪除失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async showDeliveryResume(): Promise<void> {
    await this.modalService.showDeliveryResume(this.campaignId, this.recruitmentId, this.openingId);
  }

  segmentChanged = (ev: any): void =>this.segment = ev.detail.value;

}
