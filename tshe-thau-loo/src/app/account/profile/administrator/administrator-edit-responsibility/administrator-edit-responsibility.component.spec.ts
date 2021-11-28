import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AdministratorEditResponsibilityComponent } from './administrator-edit-responsibility.component';

describe('AdministratorEditResponsibilityComponent', () => {
  let component: AdministratorEditResponsibilityComponent;
  let fixture: ComponentFixture<AdministratorEditResponsibilityComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministratorEditResponsibilityComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AdministratorEditResponsibilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
