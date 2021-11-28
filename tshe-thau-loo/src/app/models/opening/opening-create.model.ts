import { EducationType } from '../../enums/education-type.enum';

export interface IOpeningCreate {
  divisionName: string;
  jobTitle: string;
  jobDescription: string;
  workPlace: string;
  salary: string;
  requiredNumber: number;
  education: EducationType;
  workExperience: string;
  language: string;
  nationality: string;
  isAccessibility: boolean;
}
