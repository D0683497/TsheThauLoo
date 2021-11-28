import { ActivityType } from '../../enums/activity-type.enum';
import { ActivityActionType } from '../../enums/activity-action-type.enum';

export interface IActivityAttendeeSignIn {
  userId: string;
  firstId: string;
  secondId: string;
  type: ActivityType;
  action: ActivityActionType;
}
