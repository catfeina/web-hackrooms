import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StreetComponent } from './routes/street/street.component';
import { HomeComponent } from './routes/home/home.component';

const routes: Routes = [
  { path: '', redirectTo: '/street', pathMatch: 'full' },
  { path: 'street', component: StreetComponent },
  { path: 'home', component: HomeComponent },
  { path: '', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
