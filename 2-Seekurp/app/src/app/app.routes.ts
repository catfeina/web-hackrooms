import { Routes } from '@angular/router';
import { PipeComponent } from './pipe/pipe.component';
import { AuthGuard } from './auth.guard';
import { PipelineComponent } from './pipeline/pipeline.component';
import { ErrorComponent } from './error/error.component';

export const routes: Routes = [
    { path: 'pipe', component: PipeComponent },
    { path: 'pipeline', component: PipelineComponent, canActivate: [AuthGuard] },
    { path: 'error', component: ErrorComponent }
];
