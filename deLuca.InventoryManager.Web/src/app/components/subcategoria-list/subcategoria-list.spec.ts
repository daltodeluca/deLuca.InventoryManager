import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcategoriaList } from './subcategoria-list';

describe('SubcategoriaList', () => {
  let component: SubcategoriaList;
  let fixture: ComponentFixture<SubcategoriaList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubcategoriaList],
    }).compileComponents();

    fixture = TestBed.createComponent(SubcategoriaList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
