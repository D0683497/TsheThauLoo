import { ActivityType } from '../../enums/activity-type.enum';
import { ActivityActionType } from '../../enums/activity-action-type.enum';

export interface IActivityAttendeeSignIn {
  userId: string;
  activityId: string;
  type: ActivityType;
  action: ActivityActionType;
}
