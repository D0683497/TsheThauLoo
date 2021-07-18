import { ActivityType } from '../../enums/activity-type.enum';
import { ActivityActionType } from '../../enums/activity-action-type.enum';

export interface IActivityParticipantSignIn {
  firstId: string;
  secondId: string;
  type: ActivityType;
  action: ActivityActionType;
  name: string;
  contactPhone: string;
  remark: string;
}
