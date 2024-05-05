import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup} from '@angular/forms';
import { UserService } from '../../Services/UserService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit
{
  formLogin = new FormGroup({
    email: new FormControl,
    password: new FormControl
  });

  ngOnInit(): void 
  {

  }

  
  constructor(private api: UserService) {
    this.api.getUsers().subscribe((data) => {
      console.log(data);
    });
   }
   

  OnLogin(form:any){
    console.log(form);
    this.api.Login(form).subscribe((data) => {
      console.log(data);
    });
  }
}
