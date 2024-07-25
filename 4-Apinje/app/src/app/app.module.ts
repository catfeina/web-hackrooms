import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ButtonComponent } from './components/button/button.component';
import { StreetComponent } from './routes/street/street.component';
import { ArrowComponent } from './routes/arrow/arrow.component';
import { PoemComponent } from './routes/poem/poem.component';
import { TextboxComponent } from './components/textbox/textbox.component';

@NgModule({
  declarations: [
    AppComponent,
    ButtonComponent,
    StreetComponent,
    ArrowComponent,
    PoemComponent,
    TextboxComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient(
      withFetch()
    )
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
