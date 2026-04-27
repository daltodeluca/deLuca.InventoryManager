import { TestBed } from '@angular/core/testing';

import { Subcategoria } from './subcategoria';

describe('Subcategoria', () => {
  let service: Subcategoria;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Subcategoria);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
