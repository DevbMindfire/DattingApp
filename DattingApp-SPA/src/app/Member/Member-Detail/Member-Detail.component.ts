import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { User } from 'src/app/_Model/User';
import { UserService } from 'src/app/_Service/User.service';
import { AlertifyService } from 'src/app/_Service/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { AuthService } from 'src/app/_Service/Auth.service';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-Member-Detail',
  templateUrl: './Member-Detail.component.html',
  styleUrls: ['./Member-Detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  @ViewChild('MemberTabs',{static: true}) memberTabs: TabsetComponent;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService, private alertifyService: AlertifyService,
              private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.galleryOptions = [
        {
            width: '500px',
            height: '500px',
            imagePercent: 100,
            thumbnailsColumns: 4,
            imageAnimation: NgxGalleryAnimation.Slide,
            preview: false
        }
      ];

      this.galleryImages = this.GetImages();
    });

    this.route.queryParams.subscribe(params => {
      const selectedParams = params['tab'];
      this.memberTabs.tabs[selectedParams > 0 ? selectedParams : 1].active = true;
    })
  }

  GetImages(){
    const ImageUrls = [];

    for (const photo of this.user.photos){
      ImageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    }
    return ImageUrls;
  }

  SendLikes(){
    this.userService.SendLike(this.authService.decodedToken.nameid , this.user.id).subscribe(data => {
      this.alertifyService.Sucess('You have liked ' + this.user.knowsAs);
    }, error => {
      console.log(error);
      this.alertifyService.Error(error);
    });
  }

  SelectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }
}
