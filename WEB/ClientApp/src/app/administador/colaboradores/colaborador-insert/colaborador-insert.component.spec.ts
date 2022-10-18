import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColaboradorInsertComponent } from './colaborador-insert.component';

describe('ColaboradorInsertComponent', () => {
  let component: ColaboradorInsertComponent;
  let fixture: ComponentFixture<ColaboradorInsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ColaboradorInsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ColaboradorInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
