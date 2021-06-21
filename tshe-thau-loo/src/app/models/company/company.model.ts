import { IIndustrialClassification } from './industrial-classification.model';

export interface ICompany {
  id: string;
  hasLogo: boolean;
  companyConfirmed: boolean;
  registrationNumber: string;
  name: string;
  introduction: string;
  website: string;
  industrialClassifications: IIndustrialClassification[];
}
