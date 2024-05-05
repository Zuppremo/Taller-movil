import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonItem, IonInput, IonList, IonLabel, IonGrid, IonRow, IonCol, IonButton } from '@ionic/angular/standalone';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.page.html',
  styleUrls: ['./login-form.page.scss'],
  standalone: true,
  imports: [IonButton, IonCol, IonRow, IonGrid, IonLabel, IonList, IonInput, IonItem, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule]
})
export class LoginFormPage implements OnInit {
  
  constructor() { }

  ngOnInit() {
  }

}
