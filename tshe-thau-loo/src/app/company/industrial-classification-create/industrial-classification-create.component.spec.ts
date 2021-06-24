import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { IndustrialClassificationCreateComponent } from './industrial-classification-create.component';

describe('IndustrialClassificationCreateComponent', () => {
  let component: IndustrialClassificationCreateComponent;
  let fixture: ComponentFixture<IndustrialClassificationCreateComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialClassificationCreateComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(IndustrialClassificationCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
