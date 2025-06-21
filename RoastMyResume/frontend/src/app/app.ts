import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgIf],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  selectedFile: File | null = null;
  roastResult = '';

  constructor(private http: HttpClient) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  uploadFile() {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post<any>('https://localhost:5001/api/roast', formData)
      .subscribe({
        next: (res) => {
          this.roastResult = res.roast;
        },
        error: (err) => {
          console.error(err);
          alert('Error uploading file or getting roast.');
        }
      });
  }
}
