import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionRealComponent } from './asignacion-real.component';

describe('AsignacionRealComponent', () => {
  let component: AsignacionRealComponent;
  let fixture: ComponentFixture<AsignacionRealComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionRealComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionRealComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
