import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeComponent } from './pages/employee/employee.component';

const routes: Routes = [
  { path: 'add-employee', component: EmployeeComponent },
  { path: '', redirectTo: '/add-employee', pathMatch: 'full' },
  { path: '**', redirectTo: '/add-employee' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }