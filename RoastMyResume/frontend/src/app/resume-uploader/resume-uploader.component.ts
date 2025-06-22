import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-resume-uploader',
  standalone: true,
  templateUrl: './resume-uploader.component.html',
  styleUrls: ['./resume-uploader.component.css'],
  imports: [CommonModule, FormsModule]
})
export class ResumeUploaderComponent {
  selectedFile: File | null = null;
  roastResult: string = '';
  isLoading = false;

  constructor(private http: HttpClient) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files?.length) {
      this.selectedFile = input.files[0];
      this.roastResult = '';
    }
  }

  onRoast(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.isLoading = true;

    this.http.post<{ roast: string }>('http://localhost/api/roast', formData)
      .subscribe({
        next: (response) => {
          this.roastResult = response.roast; // get just the roast text
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Upload failed:', err);
          this.roastResult = 'Something went wrong! 😢';
          this.isLoading = false;
        }
      });

  }
}

