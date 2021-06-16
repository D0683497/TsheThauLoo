import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AlumnusEditVerifyFileComponent } from './alumnus-edit-verify-file.component';

describe('AlumnusEditVerifyFileComponent', () => {
  let component: AlumnusEditVerifyFileComponent;
  let fixture: ComponentFixture<AlumnusEditVerifyFileComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AlumnusEditVerifyFileComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AlumnusEditVerifyFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
