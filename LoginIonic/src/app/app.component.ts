import { Component } from '@angular/core';
import { IonApp, IonRouterOutlet, IonInput, IonGrid, IonRow, IonButton} from '@ionic/angular/standalone';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  standalone: true,
  imports: [IonApp, IonRouterOutlet, IonInput, 
    IonGrid, IonRow,  IonButton, FormsModule, ReactiveFormsModule],
})
export class AppComponent {
  constructor() {}
}
