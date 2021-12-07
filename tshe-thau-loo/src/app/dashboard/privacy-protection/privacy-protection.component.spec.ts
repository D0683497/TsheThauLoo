import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyProtectionComponent } from './privacy-protection.component';

describe('PrivacyProtectionComponent', () => {
  let component: PrivacyProtectionComponent;
  let fixture: ComponentFixture<PrivacyProtectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivacyProtectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyProtectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
