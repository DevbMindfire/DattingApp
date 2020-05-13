export interface Message {
     id: number;
     senderId: number;
     senderKnowsAs: string;
     senderPhotoUrl: string;
     recipientPhotoUrl: string;
     recipientId: number;
     recipientKnowsAs: string;
     content: string;
     isRead: boolean;
     dateRead: Date;
     messageSent: Date;
}
