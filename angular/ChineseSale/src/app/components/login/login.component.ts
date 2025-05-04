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


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ToastModule, NgClass, HttpClientModule, NgIf, ReactiveFormsModule, DividerModule, ButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [MessageService, HttpClient, UserService]
})
export class LoginComponent {
  constructor(private userService: UserService, private messageService: MessageService) {
    this.frmUser = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(18)]),
    })
  }

  frmUser: FormGroup;
  isPasswordVisible: boolean = false;
  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);

  async login(event: Event) {
    event.preventDefault();
    try {
      var res = await this.userService.login(this.frmUser.controls['firstName'].value,
        this.frmUser.controls['lastName'].value,
        this.frmUser.controls['password'].value
      )
      if (res === "success")
        this.showSuccessToast();
      else
        this.showErrorToast();
    } catch {
      this.showErrorToast();
    }
  }
  async showSuccessToast() {
    this.messageService.add({ severity: 'success', summary: 'Enter', detail: 'נכנסת בהצלחה!' });
    this.enter();
  }

  async showErrorToast() {
    this.messageService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה' });
    setTimeout(() => {
      this.messageService.clear();
    }, 3000);
  }

  enter() {
    var role = this.userService.getRole();
    setTimeout(() => {
      this.messageService.clear();
      if(role === 'Admin'){
        this.router.navigate(['admin'])
      }
      else{
        this.router.navigate(['user'])
      }
    }, 2000);
  }

  register() {
    this.router.navigate(['register'])
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
    const passwordInput: HTMLInputElement = document.getElementById('password') as HTMLInputElement;
    passwordInput.type = this.isPasswordVisible ? 'text' : 'password';
  }
}
