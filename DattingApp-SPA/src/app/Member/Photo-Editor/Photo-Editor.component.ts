import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_Model/Photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AuthService } from 'src/app/_Service/Auth.service';
import { UserService } from 'src/app/_Service/User.service';
import { AlertifyService } from 'src/app/_Service/Alertify.service';


@Component({
  selector: 'app-Photo-Editor',
  templateUrl: './Photo-Editor.component.html',
  styleUrls: ['./Photo-Editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getCurrentPhotoUrl = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: false;
  baseUrl = environment.apiUrl;
  currentPhoto: Photo;
  constructor(private authService: AuthService, private userService: UserService, private alertifyService: AlertifyService) { }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
 
  ngOnInit() {
    this.InitializeLoader();
  }
  InitializeLoader(){
    this.uploader = new FileUploader({
      url: this.baseUrl + 'User/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('Token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, xheaders) => {
      const res: Photo = JSON.parse(response);
      const photo = {
        id: res.id,
        url: res.url,
        dateAdded: res.dateAdded,
        description: res.description,
        isMain: res.isMain
      };
      this.photos.push(photo);
      if (photo.isMain){
        this.authService.ChangeMemberPhoto(photo.url);
        this.authService.currentUser.photoUrls = photo.url;
        localStorage.setItem('User', JSON.stringify(this.authService.currentUser));
      }
    };

  }
  SetMainPhoto(photo: Photo){
    this.userService.SetMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {

      this.currentPhoto = this.photos.filter(p => p.isMain === true)[0];
      this.currentPhoto.isMain = false;
      photo.isMain = true;
      this.authService.ChangeMemberPhoto(photo.url);
      this.authService.currentUser.photoUrls = photo.url;
      localStorage.setItem('User',JSON.stringify(this.authService.currentUser));

    }, error => {
      this.alertifyService.Error(error);
    });
  }

  DeletePhoto(id: number){
    this.alertifyService.Confirm('Are you sure you want to delete this photo ? ' ,() => {
      this.userService.DeletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertifyService.Sucess('Photo has been deleted');
      }, error => {
        this.alertifyService.Error(error);
      });
    });
  }
}
