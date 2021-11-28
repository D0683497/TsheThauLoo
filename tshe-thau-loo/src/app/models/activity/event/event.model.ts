import { IDocument } from '../../document/document.model';

export interface IEvent {
  id: string;
  title: string;
  content: string;
  declaration: string;
  venue: string;
  registrationStartTime: Date;
  registrationEndTime: Date;
  startTime: Date;
  endTime: Date;
  limitNumberOfPeople: number;
  enableVerify: boolean;
  enableIdentityConfirmed: boolean;
  files: IDocument[];
}
