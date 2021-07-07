import { TestBed } from '@angular/core/testing';

import { IsAdministratorGuard } from './is-administrator.guard';

describe('IsAdministratorGuard', () => {
  let guard: IsAdministratorGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(IsAdministratorGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
