import { Injectable } from '@angular/core';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  async message(message: string, icon: SweetAlertIcon): Promise<void> {
    await Swal.fire({
      icon,
      title: message,
      confirmButtonText: '確認'
    });
  }

  async notify(message: string, notify: string, icon: SweetAlertIcon): Promise<void> {
    await Swal.fire({
      icon,
      title: message,
      text: notify,
      confirmButtonText: '確認'
    });
  }

  async toast(message: string, time: number, icon: SweetAlertIcon): Promise<void> {
    await Swal.fire({
      icon,
      title: message,
      timer: time,
      timerProgressBar: true,
      toast: true,
      position: 'bottom',
      showConfirmButton: false,
      showCloseButton: true,
      width: '15rem',
      padding: '0.5rem'
    });
  }

}
