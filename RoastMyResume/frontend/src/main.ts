import { bootstrapApplication } from '@angular/platform-browser';
import { ResumeUploaderComponent } from './app/resume-uploader/resume-uploader.component';
import { provideHttpClient } from '@angular/common/http';

bootstrapApplication(ResumeUploaderComponent, {
  providers: [provideHttpClient()]
});
