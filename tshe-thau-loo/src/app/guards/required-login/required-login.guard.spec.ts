import { TestBed } from '@angular/core/testing';

import { RequiredLoginGuard } from './required-login.guard';

describe('RequiredLoginGuard', () => {
  let guard: RequiredLoginGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(RequiredLoginGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
