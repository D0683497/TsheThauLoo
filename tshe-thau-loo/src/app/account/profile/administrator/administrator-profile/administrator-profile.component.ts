import { Component, OnInit } from '@angular/core';
import { IAdministratorProfile } from '../../../../models/account/profile/administrator/administrator-profile.model';
import { BehaviorSubject } from 'rxjs';
import { AdministratorService } from '../../../../services/account/administrator/administrator.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { ActionSheetController } from '@ionic/angular';

@Component({
  selector: 'app-administrator-profile',
  templateUrl: './administrator-profile.component.html',
  styleUrls: ['./administrator-profile.component.scss'],
})
export class AdministratorProfileComponent implements OnInit {

  date = Date.now();
  profile: IAdministratorProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private administratorService: AdministratorService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private actionSheetController: ActionSheetController) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.administratorService.getProfile().subscribe(
      (res: IAdministratorProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAdministratorProfile): void {
    this.profile = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  toggleNationalId = (): boolean => this.showNationalId = !this.showNationalId;

  async editProfile(): Promise<void> {
    const actionSheet = await this.actionSheetController.create({
      header: '編輯',
      buttons: [
        {
          text: '管理員資訊',
          handler: () => {
            this.router.navigate(['/account/profile/administrator/info']);
          }
        },
        {
          text: '負責業務',
          handler: () => {
            this.router.navigate(['/account/profile/administrator/responsibility']);
          }
        },
        {
          text: '取消',
          icon: 'close',
          role: 'cancel'
        }
      ]
    });
    await actionSheet.present();
  }

}
