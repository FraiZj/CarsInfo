export interface UserClaims {
  id: number;
  roles: string[];
  email: string;
  emailVerified: boolean;
}
