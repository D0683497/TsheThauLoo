export interface IEventEdit {
  title: string;
  content: string;
  declaration: string;
  venue: string;
  registrationStartDate: Date;
  registrationStartTime: Date;
  registrationEndDate: Date;
  registrationEndTime: Date;
  startDate: Date;
  startTime: Date;
  endDate: Date;
  endTime: Date;
  limitNumberOfPeople: number;
  enableVerify: boolean;
  enableIdentityConfirmed: boolean;
}
