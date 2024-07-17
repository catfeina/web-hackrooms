import { Routes } from '@angular/router';
import { OceanComponent } from './routes/ocean/ocean.component';
import { HallComponent } from './routes/hall/hall.component';
import { TestComponent } from './routes/test/test.component';
import { HallGuard } from './guards/hall.guard';
import { ErrorComponent } from './routes/error/error.component';
import { LoreComponent } from './routes/lore/lore.component';

export const routes: Routes = [
    { path: '', redirectTo: '/ocean', pathMatch: 'full' },
    { path: 'ocean', component: OceanComponent },
    { path: 'test', component: TestComponent },
    { path: 'hall', component: HallComponent, canActivate: [HallGuard] },
    { path: 'lore', component: LoreComponent },
    { path: 'error', component: ErrorComponent },
    { path: '', component: OceanComponent }
];
