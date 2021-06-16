import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { StudentEditVerifyFileComponent } from './student-edit-verify-file.component';

describe('StudentEditVerifyFileComponent', () => {
  let component: StudentEditVerifyFileComponent;
  let fixture: ComponentFixture<StudentEditVerifyFileComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ StudentEditVerifyFileComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(StudentEditVerifyFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
