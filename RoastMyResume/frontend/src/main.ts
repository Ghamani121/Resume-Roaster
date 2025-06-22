import { bootstrapApplication } from '@angular/platform-browser';
import { ResumeUploaderComponent } from './app/resume-uploader/resume-uploader.component';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter, withHashLocation } from '@angular/router';

// If you have routes later, add them here. For now it's just to enable hash routing.
bootstrapApplication(ResumeUploaderComponent, {
  providers: [
    provideHttpClient(),
    provideRouter([], withHashLocation())
  ]
});
