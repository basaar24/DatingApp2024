import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  // usersFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();
  model: any = {};

  register(): void {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
}
