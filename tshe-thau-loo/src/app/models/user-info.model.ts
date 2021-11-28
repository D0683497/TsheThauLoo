import { RoleType } from '../enums/role-type.enum';

export interface IUserInfo {
  nameId: string; // 使用者識別碼
  uniqueName: string; // 使用者名稱
  email: string;
  sid: string;
  role: RoleType;
  jti: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}

export class UserInfo implements IUserInfo {

  uniqueName: string;
  email: string;
  nameId: string;
  sid: string;
  role: RoleType;
  jti: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;

  constructor(token: any) {
    this.uniqueName = token.unique_name;
    this.email = token.email;
    this.nameId = token.nameid;
    this.sid = token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
    this.role = token.role;
    this.jti = token.jti;
    this.nbf = token.nbf;
    this.exp = token.exp;
    this.iat = token.iat;
    this.iss = token.iss;
    this.aud = token.aud;
  }

}
