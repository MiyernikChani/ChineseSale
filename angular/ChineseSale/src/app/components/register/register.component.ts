import { Component } from '@angular/core';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { FormControl } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { NgClass } from '@angular/common';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ToastModule, NgClass, HttpClientModule, NgIf, ReactiveFormsModule, DividerModule, ButtonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [MessageService, HttpClient, UserService]
})
export class RegisterComponentComponent {
  constructor(private userService: UserService, private messageService: MessageService) {
    this.frmUser = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]),
      phone: new FormControl('', [Validators.required, Validators.minLength(9), Validators.maxLength(10)]),
      mail: new FormControl('', [Validators.required, Validators.email]),
      address: new FormControl('')
    })
  }

  frmUser: FormGroup;
  isPasswordVisible: boolean = false;
  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);

  async register(role:string) {
    var user = new User();
    user.firstName = this.frmUser.controls['firstName'].value;
    user.lastName = this.frmUser.controls['lastName'].value;
    user.password = this.frmUser.controls['password'].value;
    user.phone = this.frmUser.controls['phone'].value;
    user.mail = this.frmUser.controls['mail'].value;
    user.address = this.frmUser.controls['address'].value;
    user.role = role;

    this.userService.register(user).subscribe({
      next: (response) => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
        if (response.status === 200) {
          this.showSuccessToast();
        }
      },
      error: (error) => {
        console.error('שגיאה בהרשמה :', error);
        if(error.status === 200){
          this.showSuccessToast();
        }
        else if(error.status === 409){
          this.messageService.add({ severity: 'error', summary: 'add', detail: 'משתמש קיים!' });
        }
        else{
          this.messageService.add({ severity: 'error', summary: 'add', detail: 'שגיאה בהרשמה' });
        }
      }
    });
  }
  async showSuccessToast() {
    this.messageService.add({ severity: 'success', summary: 'add', detail: 'נרשמת בהצלחה!' });
    this.enter();
  }

  enter() {
    setTimeout(() => {
      this.messageService.clear();
      this.router.navigate(['login'])
    }, 2000);
  }

  login(){
    this.router.navigate(['login'])
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
    const passwordInput: HTMLInputElement = document.getElementById('password') as HTMLInputElement;
    passwordInput.type = this.isPasswordVisible ? 'text' : 'password';
  }
}
