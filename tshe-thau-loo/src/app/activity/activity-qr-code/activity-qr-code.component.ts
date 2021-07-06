import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { toDataURL } from 'qrcode';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-activity-qr-code',
  templateUrl: './activity-qr-code.component.html',
  styleUrls: ['./activity-qr-code.component.scss'],
})
export class ActivityQrCodeComponent implements OnInit {

  @Input() title: string;
  @Input() uri: string;
  date = Date.now();
  data: string;

  constructor(private modalController: ModalController) { }

  async ngOnInit(): Promise<void> {
    this.data = await toDataURL(this.uri, {errorCorrectionLevel: 'H'});
  }

  async dismiss(agree: boolean): Promise<void> {
    await this.modalController.dismiss(agree);
  }

  async download(): Promise<void> {
    saveAs(this.data, `${this.title}.png`);
  }

}
