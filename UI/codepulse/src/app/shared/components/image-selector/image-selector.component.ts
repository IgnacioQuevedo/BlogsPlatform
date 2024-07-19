import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ImageService } from './image.service';
import { Observable, Subscription } from 'rxjs';
import { BlogImage } from '../../models/blog-image.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.css']
})
export class ImageSelectorComponent implements OnDestroy, OnInit
{

  private file? : File;
  fileName : string = '';
  title : string = '';
  uploadImageSubscription? : Subscription;
  images$? : Observable<BlogImage[]>

  @ViewChild('form',{ static: false}) imageUploadForm?: NgForm

  constructor(private imageService : ImageService)
  {
    
  }

  onFileUploadChange(event : Event) : void 
  {
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];
  }
  
  selectedImage(image: BlogImage): void
  {
    this.imageService.selectImage(image);
  }

  uploadImage() : void
  {

    if(this.file && this.fileName !=='' && this.title !== '')
    {
      //Service
      this.uploadImageSubscription = this.imageService.uploadImage(this.file,this.fileName,this.title)
      .subscribe({
        next: (response) => {
          this.imageUploadForm?.resetForm();
          this.getImages();
        }
      })

    }

  }


  ngOnInit(): void 
  {
     this.getImages();
  }
  
  ngOnDestroy(): void
  {
    this.uploadImageSubscription?.unsubscribe;
  }

  private getImages()
  {
    this.images$ = this.imageService.getImages();
  }


}
