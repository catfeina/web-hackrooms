import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RoadComponent } from './routes/road/road.component';
import { ButtonComponent } from './components/button/button.component';
import { TextboxComponent } from './components/textbox/textbox.component';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { StreetComponent } from './routes/street/street.component';
import { BuildingComponent } from './routes/building/building.component';
import { HouseComponent } from './routes/house/house.component';

@NgModule({
  declarations: [
    AppComponent,
    RoadComponent,
    ButtonComponent,
    TextboxComponent,
    StreetComponent,
    BuildingComponent,
    HouseComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [
    provideHttpClient(
      withFetch()
    )
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
