export interface IRecruitmentCampaignCreate {
  title: string;
  content: string;
  startDate: Date;
  startTime: Date;
  endDate: Date;
  endTime: Date;
  enableReview: boolean;
}
