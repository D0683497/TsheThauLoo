import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyRoutingModule } from './company-routing.module';
import { IonicModule } from '@ionic/angular';
import { CompanyHomeComponent } from './company-home/company-home.component';
import { CompanyCreateComponent } from './company-create/company-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import * as QuillBlotFormatter from 'quill-blot-formatter';
import { CompanyDisplayComponent } from './company-display/company-display.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { IndustrialClassificationCreateComponent } from './industrial-classification-create/industrial-classification-create.component';
import { IndustrialClassificationEditComponent } from './industrial-classification-edit/industrial-classification-edit.component';
import { CompanyListComponent } from './company-list/company-list.component';

@NgModule({
  declarations: [
    CompanyHomeComponent,
    CompanyCreateComponent,
    CompanyDisplayComponent,
    CompanyEditComponent,
    IndustrialClassificationCreateComponent,
    IndustrialClassificationEditComponent,
    CompanyListComponent
  ],
  imports: [
    CommonModule,
    CompanyRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot({
      modules: {
        syntax: false, // 程式碼語法檢測
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }], // 標題大小
          ['bold', 'italic', 'underline', 'strike'],
          [{ list: 'ordered'}, { list: 'bullet' }],
          [{ align: [] }],
          [{ indent: '-1'}, { indent: '+1' }],
          [{ color: [] }, { background: [] }],
          ['blockquote', 'code-block'],
          ['link', 'image'],
          ['clean'],
        ],
        blotFormatter: {}
      },
      customModules: [{
        implementation: QuillBlotFormatter.default,
        path: 'modules/blotFormatter'
      }],
    })
  ]
})
export class CompanyModule { }
