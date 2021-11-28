import { EducationType } from '../../enums/education-type.enum';
import { IQualification } from './qualification.model';
import { IFaculty } from './faculty.model';

export interface IOpening {
  id: string;
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
  qualifications: IQualification[];
  faculties: IFaculty[];
}
