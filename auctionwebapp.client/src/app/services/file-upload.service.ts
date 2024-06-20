import { HttpClient, HttpEvent, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class FileUploadService {
  private baseUrl = 'http://localhost:5234';

  constructor(private http: HttpClient) { }

  upload(file: File, lotId: number, imageId: number): Observable<HttpEvent<any>> {
    let fileBlob = new Blob([file]);
    const formData: FormData = new FormData();
    formData.append('Image', fileBlob, lotId.toString() + "-" + imageId.toString());

    const req = new HttpRequest('POST', `${this.baseUrl}/images/upload`, formData, {
      reportProgress: true,
      responseType: 'json',
    });

    return this.http.request(req);
  }

  getFiles(): Observable<any> {
    return this.http.get(`${this.baseUrl}/files`);
  }
}
