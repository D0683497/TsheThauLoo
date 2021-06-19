import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AdministratorCreateResponsibilityComponent } from './administrator-create-responsibility.component';

describe('AdministratorCreateResponsibilityComponent', () => {
  let component: AdministratorCreateResponsibilityComponent;
  let fixture: ComponentFixture<AdministratorCreateResponsibilityComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministratorCreateResponsibilityComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AdministratorCreateResponsibilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
