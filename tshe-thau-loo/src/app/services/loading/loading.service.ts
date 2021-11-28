import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  loading: HTMLIonLoadingElement;

  constructor(private loadingController: LoadingController) { }

  async start(message: string): Promise<void> {
    this.loading = await this.loadingController.create({message});
    await this.loading.present();
  }

  async end(): Promise<void> {
    await this.loading.dismiss();
  }

}
