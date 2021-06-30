import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { RoleType } from '../../enums/role-type.enum';
import { Router } from '@angular/router';
import { ActionSheetController } from '@ionic/angular';

@Component({
  selector: 'app-activity-home',
  templateUrl: './activity-home.component.html',
  styleUrls: ['./activity-home.component.scss'],
})
export class ActivityHomeComponent implements OnInit {

  date = Date.now();
  type = RoleType;

  constructor(
    public authService: AuthService,
    private actionSheetController: ActionSheetController,
    private router: Router) { }

  ngOnInit(): void {}

  async create(): Promise<void> {
    const actionSheet = await this.actionSheetController.create({
      header: '建立活動',
      buttons: [
        {
          text: '一般活動',
          handler: () => {
            this.router.navigate(['/act/event/create']);
          }
        },
        {
          text: '系列活動',
          handler: () => {
            this.router.navigate(['/act/campaign/create']);
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
