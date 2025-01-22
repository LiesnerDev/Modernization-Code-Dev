import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = '/api/employee';

  constructor(private http: HttpClient) {}

  addEmployee(employee: any): Observable<any> {
    return this.http.post(this.apiUrl, employee);
  }
}