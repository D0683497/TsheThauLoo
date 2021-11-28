import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification/notification.service';
import { SweetAlertIcon } from '../enums/sweet-alert-icon.enum';
import { IServerError } from '../models/error/server-error.model';

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private notificationService: NotificationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 401:
            this.router.navigate(['/account/login']).then();
            this.notificationService.message('請先登入', SweetAlertIcon.warning).then();
            break;
          case 500:
            const e: IServerError = error.error;
            this.notificationService.notify(e.title, e.detail, SweetAlertIcon.error).then();
            break;
          default:
            break;
        }
        return throwError(error);
      })
    );
  }

}
