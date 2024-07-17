import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { hallGuard } from './hall.guard';

describe('hallGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => hallGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
