import { Injectable } from "@angular/core";
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../Member/Member-Edit/Member-Edit.component';

@Injectable()

export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent>{
     canDeactivate(component: MemberEditComponent){
          if (component.editForm.dirty){
               return confirm('Are sure you want to continue . Any unsaved changes will be lost')
          }
          return true;
     }
}