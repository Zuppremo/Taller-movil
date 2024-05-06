import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ILoginRequest } from 'src/app/Models/ILoginRequest';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonItem, IonInput, IonList, IonLabel, IonGrid, IonRow, IonCol, IonButton, IonInputPasswordToggle, IonAvatar, IonText } from '@ionic/angular/standalone';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.page.html',
  styleUrls: ['./login-form.page.scss'],
  standalone: true,
  imports: [IonText, IonButton, IonCol, IonRow, IonGrid, IonLabel, IonList, 
    IonInput, IonItem, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, IonInputPasswordToggle, IonAvatar, IonText],
})
export class LoginFormPage implements OnInit {
  email: string = "";
  password: string = "";

  constructor(private api: UserService) {
    this.api.getUsers().subscribe((data) => {
      console.log(data);
    });
   }

  ngOnInit() {
  }

  OnSubmitLogin(email: any, password: any) {
    console.log('Form submitted');
    console.log('email submitted');
    this.email = email;
    this.password = password;
    let ILoginRequest : ILoginRequest = {
      email: this.email,
      password: this.password
    }
    this.api.Login(ILoginRequest).subscribe((data) => {
      console.log(data);
    });
    console.log(this.email);
    console.log(this.password);

  }

}
