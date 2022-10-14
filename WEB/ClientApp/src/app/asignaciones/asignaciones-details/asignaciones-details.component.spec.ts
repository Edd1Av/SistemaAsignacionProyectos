import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesDetailsComponent } from './asignaciones-details.component';

describe('AsignacionesDetailsComponent', () => {
  let component: AsignacionesDetailsComponent;
  let fixture: ComponentFixture<AsignacionesDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
