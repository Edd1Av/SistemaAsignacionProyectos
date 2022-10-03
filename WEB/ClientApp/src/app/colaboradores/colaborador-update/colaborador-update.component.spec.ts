import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColaboradorUpdateComponent } from './colaborador-update.component';

describe('ColaboradorUpdateComponent', () => {
  let component: ColaboradorUpdateComponent;
  let fixture: ComponentFixture<ColaboradorUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ColaboradorUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ColaboradorUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
