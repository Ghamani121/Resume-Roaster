import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.html',
  imports: [CommonModule, FormsModule, HttpClientModule],
})
export class AppComponent {
  selectedFile: File | null = null;
  roastResult: string = '';

  constructor(private http: HttpClient) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  uploadResume() {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('File', this.selectedFile); // 'File' must match the backend model

    this.http
      .post<any>('https://localhost:5001/api/Roast', formData)
      .subscribe({
        next: (response) => {
          this.roastResult = response.roast;
        },
        error: (err) => {
          this.roastResult = `Error: ${err.error?.message || 'Upload failed'}`;
        },
      });
  }
}
