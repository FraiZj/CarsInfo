export interface JwtPayload {
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[] | string;
  Id: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
  'EmailVerified'?: string;
}
