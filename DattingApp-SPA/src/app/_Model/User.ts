import { Photo } from './Photo';

export interface User {
     id: number;
     userName: string;
     knowsAs: string;
     age: number;
     gender: string;
     created: Date;
     lastActive: Date;
     photoUrls: string;
     city: string;
     country: string;
     interests?: string;
     introduction?: string;
     lookingFor?: string;
     photos?: Photo[];
}
