import { ComponentFixture, TestBed, fakeAsync, tick, flush } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { EmployeeComponent } from './employee.component';
import { EmployeeService } from '../../core/services/employee.service';
import { By } from '@angular/platform-browser';

describe('EmployeeComponent', () => {
  let component: EmployeeComponent;
  let fixture: ComponentFixture<EmployeeComponent>;
  let mockEmployeeService: jasmine.SpyObj<EmployeeService>;

  beforeEach(async () => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['addEmployee']);

    await TestBed.configureTestingModule({
      declarations: [ EmployeeComponent ],
      imports: [ FormsModule ],
      providers: [
        { provide: EmployeeService, useValue: employeeServiceSpy }
      ]
    })
    .compileComponents();

    mockEmployeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  function setFormValues(employeeId: number, name: string, age: number, address: string) {
    component.employeeId = employeeId;
    component.name = name;
    component.age = age;
    component.address = address;
  }

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Cenário 01: Inserção de Dados Válidos de Funcionário', fakeAsync(() => {
    setFormValues(1234, 'João Silva', 30, 'Rua das Flores, 123');

    const mockResponse = 'Registro de funcionário adicionado.';
    mockEmployeeService.addEmployee.and.returnValue(of(mockResponse));

    // Simulate form submission
    const form = {
      invalid: false,
      resetForm: jasmine.createSpy('resetForm')
    } as any;
    component.onSubmit(form);
    tick();

    expect(mockEmployeeService.addEmployee).toHaveBeenCalledWith({
      employeeId: 1234,
      name: 'João Silva',
      age: 30,
      address: 'Rua das Flores, 123'
    });
    expect(component.successMessage).toBe(mockResponse);
    expect(component.errorMessage).toBe('');
    expect(form.resetForm).toHaveBeenCalled();
    flush();
  }));

  it('Cenário 02: Inserção de ID de Funcionário Inválido - Menos de 4 dígitos', () => {
    setFormValues(123, 'João Silva', 30, 'Rua das Flores, 123');

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 02: Inserção de ID de Funcionário Inválido - Mais de 4 dígitos', () => {
    setFormValues(12345, 'João Silva', 30, 'Rua das Flores, 123');

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 03: Inserção de Nome de Funcionário Inválido - Mais de 20 caracteres', () => {
    const longName = 'Nome do Funcionário com mais de vinte caracteres';
    setFormValues(1234, longName, 30, 'Rua das Flores, 123');

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 04: Inserção de Idade de Funcionário Inválida - Menos de 2 dígitos', () => {
    setFormValues(1234, 'João Silva', 5, 'Rua das Flores, 123');

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 04: Inserção de Idade de Funcionário Inválida - Mais de 2 dígitos', () => {
    setFormValues(1234, 'João Silva', 123, 'Rua das Flores, 123');

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 05: Inserção de Endereço de Funcionário Inválido - Mais de 30 caracteres', () => {
    const longAddress = 'Endereço do Funcionário que excede trinta caracteres';
    setFormValues(1234, 'João Silva', 30, longAddress);

    const form = {
      invalid: true
    } as any;
    component.onSubmit(form);

    expect(mockEmployeeService.addEmployee).not.toHaveBeenCalled();
  });

  it('Cenário 06: Falha na Escrita no Banco de Dados', fakeAsync(() => {
    setFormValues(1234, 'João Silva', 30, 'Rua das Flores, 123');

    const mockError = { error: 'Falha na inserção do registro' };
    mockEmployeeService.addEmployee.and.returnValue(throwError(mockError));

    const form = {
      invalid: false,
      resetForm: jasmine.createSpy('resetForm')
    } as any;
    component.onSubmit(form);
    tick();

    expect(mockEmployeeService.addEmployee).toHaveBeenCalledWith({
      employeeId: 1234,
      name: 'João Silva',
      age: 30,
      address: 'Rua das Flores, 123'
    });
    expect(component.errorMessage).toBe(mockError.error);
    expect(component.successMessage).toBe('');
    expect(form.resetForm).not.toHaveBeenCalled();
    flush();
  }));
});
