import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit
{
  formLogin = new FormGroup({
    userName: new FormControl,
    password: new FormControl
  });

  ngOnInit(): void 
  {

  }

  OnLogin(form:any){
    console.log(form);
  }
}
