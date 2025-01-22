import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { EmployeeService } from '../../core/services/employee.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent {
  employeeId: number | null = null;
  name: string = '';
  age: number | null = null;
  address: string = '';
  successMessage: string = '';
  errorMessage: string = '';

  constructor(private employeeService: EmployeeService) {}

  onSubmit(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    const employee = {
      employeeId: this.employeeId,
      name: this.name,
      age: this.age,
      address: this.address
    };

    this.employeeService.addEmployee(employee).subscribe({
      next: (response) => {
        this.successMessage = response;
        this.errorMessage = '';
        form.resetForm();
      },
      error: (error) => {
        this.errorMessage = error.error;
        this.successMessage = '';
      }
    });
  }
}