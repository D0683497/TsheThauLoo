import { TestBed } from '@angular/core/testing';

import { AccountRedirectGuard } from './account-redirect.guard';

describe('AccountRedirectGuard', () => {
  let guard: AccountRedirectGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AccountRedirectGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
