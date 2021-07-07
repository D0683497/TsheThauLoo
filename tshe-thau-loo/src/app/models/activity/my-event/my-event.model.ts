import { AttendeeStatusType } from '../../../enums/attendee-status-type.enum';

export interface IMyEvent {
  id: string;
  title: string;
  venue: string;
  registrationStartTime: Date;
  registrationEndTime: Date;
  startTime: Date;
  endTime: Date;
  status: AttendeeStatusType;
}
