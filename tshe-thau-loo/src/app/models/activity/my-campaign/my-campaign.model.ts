import { IMyGeneralCampaign } from './my-general-campaign.model';

export interface IMyCampaign {
  id: string;
  title: string;
  content: string;
  startTime: Date;
  endTime: Date;
  generalCampaigns: IMyGeneralCampaign[];
}
