using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TalenteenModels;
using TalentSteenModels;

namespace TalentSteenAPI_V1.Controllers
{
    public class RecruiterController : ApiController
    {
        [HttpPost]
        public Response CreateCV([FromBody]CVModel objCVIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            DateTime dt;
            DateTime.TryParseExact(objCVIn.DOB,
                            "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                                out dt);
            decimal? i = obj.USP_CreateCV(objCVIn.RecruiterId, objCVIn.CreatedBy, objCVIn.FullName, int.Parse(objCVIn.NationalityId), objCVIn.Gender, dt,
                objCVIn.MaritalStatus, objCVIn.PrimaryContact, objCVIn.SecondaryContact, objCVIn.EmailAddress, bool.Parse(objCVIn.WillingToRelocate.ToString()), bool.Parse(objCVIn.WillingToTravel.ToString()), objCVIn.AvailabilityTimeToJoin, objCVIn.SkillSet, objCVIn.Others).FirstOrDefault();
            string res = i.ToString();
            if (decimal.Parse(i.ToString()) > 0)
            {
                if (objCVIn.CVExperienceList.Count > 0)
                {
                    foreach (CVExperienceModel objExp in objCVIn.CVExperienceList)
                    {
                        decimal? j = obj.USP_CreateCVExperience(Convert.ToInt32(i), int.Parse(objExp.YearOfJoining.ToString()), objExp.CompanyName, int.Parse(objExp.IndustryId.ToString()), int.Parse(objExp.CompanyLocationId.ToString()), objExp.Designation,
                            int.Parse(objExp.FunctionalAreaId.ToString()), objExp.ReportingTo, decimal.Parse(objExp.Salary), objExp.Responsibilities, objExp.MoreDetails).FirstOrDefault();
                    }
                }

                if (objCVIn.CVEducationList.Count > 0)
                {
                    foreach (CVEducation objEdu in objCVIn.CVEducationList)
                    {
                        decimal? j = obj.USP_CreateCVCertification(Convert.ToInt32(i), int.Parse(objEdu.YearOfPassing), objEdu.Education, objEdu.Major, objEdu.University, int.Parse(objEdu.LocationId)).FirstOrDefault();
                    }
                }

                objResponse.IsSuccess = true;
                objResponse.ErrorMessage = "Record updated successfully";
            }
            else
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
        [HttpPost]
        public RecruiterJobListResponse FetchNewJobsOfRecruiter([FromBody]RecruiterJobListRequest objRecruiterJobListRequestIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            RecruiterJobListResponse objResponse = new RecruiterJobListResponse();
            List<USP_GetNewJobsOfRecruiter_Result> objList = new List<TalentSteenAPI_V1.USP_GetNewJobsOfRecruiter_Result>();
            objList = obj.USP_GetNewJobsOfRecruiter(objRecruiterJobListRequestIn.UserId, objRecruiterJobListRequestIn.RecruiterId, objRecruiterJobListRequestIn.Limit1, objRecruiterJobListRequestIn.Limit2, objRecruiterJobListRequestIn.JobStatus).ToList();
            if (objList.Count > 0)
            {
                var listObj = objList.Select(x => new RecruiterJobModel()
                {
                    JobId = x.JobId,
                    EmployerName = x.EmployerName,
                    BillingAmount = x.RewardOffered.ToString(),
                    ExpiresOn = DateTime.Parse(x.EndDate.ToString()),
                    JobStatus = x.JobStatusName,
                    JobTitle = x.JobTitle,
                    PaymentTerms = "60 days after a candidate joins",
                    ExpairyDays = DateTime.Parse(x.EndDate.ToString()).Subtract(DateTime.Now).Days,
                    PostedOn = DateTime.Parse(x.CreatedOn.ToString()),
                    Reward = x.RewardOffered.ToString(),
                    CVSubmittedList = new List<CVSubmitted>(),
                    RowNo = int.Parse(x.RowNo.ToString()),
                    JobFunctionName = x.JobFunctionName,
                    Nationality = x.CountryName,
                    JobLocation = x.LocationName,
                    Education = x.Education,
                    MinYearsOfExperience = x.MinimumYearsOfExperience,
                    MaxYearsOfExperience = x.MaximumYearsOfExperience,
                    SalaryRangeFrom = float.Parse(x.SalaryRangeFrom.ToString()),
                    SalaryRangeTo = float.Parse(x.SalaryRangeTo.ToString()),
                    JobDescription = x.JobDescription,
                    GenderName = x.GenderName,
                    IndustryType = x.IndustryTypeName,
                    SearchTags = x.SearchTags,
                    AgeFrom = int.Parse(x.AgeFrom.ToString()),
                    AgeTo = int.Parse(x.AgeTo.ToString()),
                    Certifications=x.Certifications
                }).ToList();
                objResponse.JobListing = (List<RecruiterJobModel>)listObj;
                objResponse.TotalRecord = int.Parse(objList[0].TotalRecord.ToString());
            }
            return objResponse;
        }
        [HttpPost]
        public RecruiterJobListResponse FetchRecruiterJobList([FromBody]RecruiterJobListRequest objRecruiterJobListRequestIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            RecruiterJobListResponse objResponse = new RecruiterJobListResponse();
            List<USP_GetRecruiterJobs_Result> objList = new List<TalentSteenAPI_V1.USP_GetRecruiterJobs_Result>();
            objList = obj.USP_GetRecruiterJobs(objRecruiterJobListRequestIn.UserId, objRecruiterJobListRequestIn.RecruiterId, objRecruiterJobListRequestIn.Limit1, objRecruiterJobListRequestIn.Limit2, objRecruiterJobListRequestIn.JobStatus).ToList();
            if (objList.Count > 0)
            {
                var listObj = objList.Select(x => new RecruiterJobModel()
                {
                    JobId = x.JobId,
                    EmployerName = x.EmployerName,
                    BillingAmount = x.RewardOffered.ToString(),
                    ExpiresOn = DateTime.Parse(x.EndDate.ToString()),
                    JobStatus = x.JobStatusName,
                    JobTitle = x.JobTitle,
                    PaymentTerms = "60 days after a candidate joins",
                    ExpairyDays = DateTime.Parse(x.EndDate.ToString()).Subtract(DateTime.Now).Days,
                    PostedOn = DateTime.Parse(x.CreatedOn.ToString()),
                    Reward = x.RewardOffered.ToString(),
                    CVSubmittedList = new List<CVSubmitted>(),
                    RowNo = int.Parse(x.RowNo.ToString()),
                    JobFunctionName = x.JobFunctionName,
                    Nationality = x.CountryName,
                    JobLocation = x.LocationName,
                    Education = x.Education,
                    MinYearsOfExperience = x.MinimumYearsOfExperience,
                    MaxYearsOfExperience = x.MaximumYearsOfExperience,
                    SalaryRangeFrom = float.Parse(x.SalaryRangeFrom.ToString()),
                    SalaryRangeTo = float.Parse(x.SalaryRangeTo.ToString()),
                    JobDescription = x.JobDescription,
                    GenderName=x.GenderName,
                    IndustryType=x.IndustryTypeName,
                    SearchTags=x.SearchTags,
                    AgeFrom=int.Parse(x.AgeFrom.ToString()),
                    AgeTo= int.Parse(x.AgeTo.ToString())
                }).ToList();
                objResponse.JobListing = (List<RecruiterJobModel>)listObj;
                if (objResponse.JobListing.Count > 0)
                {
                    foreach (RecruiterJobModel RJ in objResponse.JobListing)
                    {
                        List<USP_SubmittedCV_Result> objInnerList = new List<TalentSteenAPI_V1.USP_SubmittedCV_Result>();
                        objInnerList = obj.USP_SubmittedCV(Convert.ToInt32(RJ.JobId.ToString())).ToList();
                        if (objInnerList.Count > 0)
                        {
                            var listInnerObj = objInnerList.Select(x => new CVSubmitted()
                            {
                                CandidateName = x.FullName,
                                CVStatusId = x.CVStatusId,
                                Status = x.CVStatusName,
                                SubmittedSince = Convert.ToInt16(x.SubmittedSince)
                            }).ToList();
                            RJ.CVSubmittedList = (List<CVSubmitted>)listInnerObj;
                        }
                    }
                }
                objResponse.TotalRecord = int.Parse(objList[0].TotalRecord.ToString());
            }
            return objResponse;
        }

        [HttpPost]
        public RecruiterCVListResponse FetchRecruiterCvs([FromBody]RecruiterCVListRequest objRecruiterJobListRequestIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            RecruiterCVListResponse objResponse = new RecruiterCVListResponse();
            List<USP_GetRecruiterCVs_Result> objList = new List<TalentSteenAPI_V1.USP_GetRecruiterCVs_Result>();
            objList = obj.Usp_GetRecruiterCVs(objRecruiterJobListRequestIn.UserId, objRecruiterJobListRequestIn.RecruiterId, objRecruiterJobListRequestIn.Limit1, objRecruiterJobListRequestIn.Limit2).ToList();
            if (objList.Count > 0)
            {
                var listObj = objList.Select(x => new RecruiterCV()
                {
                    Age = x.Age.ToString(),
                    CompanyName = x.CompanyName,
                    CountryName = x.CountryName,
                    CVId = x.CVId,
                    Designation = x.Designation,
                    FullName = x.FullName,
                    GenderName = x.GenderName,
                    MaritalStatusName = x.MaritalStatusName,
                    SkillSet = x.SkillSet,
                    RowNo = int.Parse(x.RowNo.ToString())
                }).ToList();
                objResponse.CVListing = (List<RecruiterCV>)listObj;
                objResponse.TotalRecord = int.Parse(objList[0].TotalRecord.ToString());
            }
            return objResponse;
        }

        [HttpPost]
        public CVModel FetchCVDetails([FromBody]int cvId)
        {
            CVModel objCV = new CVModel();
            using (var context = new TalentSteenEntities())
            {
                var query = from cv in context.CV
                            join c in context.Country on cv.CountryId equals c.CountryId
                            join g in context.Gender on cv.GenderId equals g.GenderId
                            join m in context.MaritalStatus on cv.MaritalStatusId equals m.MaritalStatusId
                            where cv.CVId == cvId
                            select new CVModel
                            {
                                Age = System.Data.Entity.DbFunctions.DiffDays(cv.DOB, DateTime.Now).Value / 365,
                                AvailabilityTimeToJoin = cv.NoticePeriod,
                                CountryName = c.CountryName,
                                CreatedBy = cv.CreatedBy,
                                EmailAddress = cv.EmailId,
                                FullName = cv.FullName,
                                Gender = cv.GenderId,
                                GenderName = g.GenderName,
                                MaritalStatus = cv.MaritalStatusId,
                                MaritalStatusName = m.MaritalStatusName,
                                Others = cv.Others,
                                PrimaryContact = cv.PrimaryContactNumber,
                                RecruiterId = cv.RecruiterId,
                                SecondaryContact = cv.SecondaryContactNumber,
                                SkillSet = cv.SkillSet,
                                WillingToRelocate = cv.IsWillingToRelocate == true ? "YES" : "NO",
                                WillingToTravel = cv.IsWillingToTravel == true ? "YES" : "NO"
                            };
                //.Where(s => s.CVId == cvId)
                //.FirstOrDefault<CV>();
                //objCV = new CVModel()
                //{
                //    Age = query.,
                //    AvailabilityTimeToJoin = query.NoticePeriod,
                //    CreatedBy = query.CreatedBy,
                //    EmailAddress = query.EmailId,
                //    FullName = query.FullName,
                //    Gender = query.GenderId,
                //    MaritalStatus = query.MaritalStatusId,
                //    NationalityId = query.CountryId.ToString(),
                //    Others = query.Others,
                //    PrimaryContact = query.PrimaryContactNumber,
                //    RecruiterId = query.RecruiterId,
                //    SecondaryContact = query.SecondaryContactNumber,
                //    SkillSet = query.SkillSet,
                //    WillingToRelocate = query.IsWillingToRelocate.ToString(),
                //    WillingToTravel = query.IsWillingToTravel.ToString()
                //};
                objCV = (CVModel)query.SingleOrDefault();
                if (objCV != null)
                {
                    var experienceList = from cvExp in context.CVExperience
                                         where cvExp.CVId == cvId
                                         select new CVExperienceModel
                                         {
                                             CompanyName = cvExp.CompanyName,
                                             CompanyLocationId = cvExp.CompanyLocationId.ToString(),
                                             Designation = cvExp.Designation,
                                             MoreDetails = cvExp.MoreDetails,
                                             Salary = cvExp.Salary.ToString(),
                                             YearOfJoining = cvExp.YearOfJoining.ToString()
                                         };
                    objCV.CVExperienceList = (List<CVExperienceModel>)experienceList.ToList();

                    var educationList = from cvEdu in context.CVCertification
                                        where cvEdu.CVId == cvId
                                        select new CVEducation
                                        {
                                            Education = cvEdu.Certification,
                                            Major = cvEdu.Major,
                                            University = cvEdu.University,
                                            YearOfPassing = cvEdu.YearOfPassing.ToString()
                                        };
                    objCV.CVEducationList = (List<CVEducation>)educationList.ToList();

                    var recruiterEmployers = from recInd in context.RecruiterIndustries
                                             join E in context.Employer on recInd.IndustryTypeId equals E.IndustryTypeId
                                             join j in context.Jobs on E.EmployerId equals j.EmployerId
                                             join js in context.JobStatus on j.JobStatusId equals js.JobStatusId
                                             where recInd.RecruiterId == objCV.RecruiterId && js.JobStatusId == (int)JobStatusModel.OPEN
                                             select new EmployerRegistration
                                             {
                                                 EmployerId = E.EmployerId,
                                                 CompanyName = E.EmployerName
                                             };
                    var recEmp = recruiterEmployers.GroupBy(c => c.EmployerId).Select(group => group.FirstOrDefault());
                    objCV.RecruiterEmployers = (List<EmployerRegistration>)recEmp.Take(10).ToList();

                    var recruiterJobs = from recInd in context.RecruiterIndustries
                                        join E in context.Employer on recInd.IndustryTypeId equals E.IndustryTypeId
                                        join j in context.Jobs on E.EmployerId equals j.EmployerId
                                        join js in context.JobStatus on j.JobStatusId equals js.JobStatusId
                                        where recInd.RecruiterId == objCV.RecruiterId && js.JobStatusId == (int)JobStatusModel.OPEN
                                        select new Job
                                        {
                                            JobId = j.JobId,
                                            JobTitle = j.JobTitle
                                        };
                    objCV.RecruiterJobs = (List<Job>)recruiterJobs.ToList();
                }
            }
            CommonController objCommon = new CommonController();
            objCV.CVStatusList = objCommon.GetCVStatusList((int)CvStatusEnum.SentOrRecieved);
            return objCV;
        }
        [HttpPost]
        public List<Job> FetchEmployerjobsById([FromBody]int employerId)
        {
            using (var context = new TalentSteenEntities())
            {
                var recruiterJobs = from E in context.Employer 
                                    join j in context.Jobs on E.EmployerId equals j.EmployerId
                                    join js in context.JobStatus on j.JobStatusId equals js.JobStatusId
                                    where E.EmployerId== employerId && js.JobStatusId == (int)JobStatusModel.OPEN
                                    select new Job
                                    {
                                        JobId = j.JobId,
                                        JobTitle = j.JobTitle
                                    };
                return (List<Job>)recruiterJobs.ToList();
            }
        }
        [HttpPost]
        public Response SubmitCV([FromBody]JobCVModel objJobCVIn)
        {
            Response objResponse = new Response();
            JobCV objJobCV = new JobCV()
            {
                CVStatusId = objJobCVIn.CVStatusId,
                JobId = objJobCVIn.JobId,
                CVId = objJobCVIn.CVId,
                SubmittedBy = objJobCVIn.SubmittedBy,
                SubmittedOn = objJobCVIn.SubmittedOn
            };
            try
            {
                TalentSteenEntities obj = new TalentSteenEntities();
                obj.JobCV.Add(objJobCV);
                obj.SaveChanges();
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "cv uploaded against job successfully";
                
            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
        [HttpPost]
        public RecruiterCV CheckCVExists([FromBody]string emailId)
        {
            RecruiterCV objCV = new RecruiterCV();
            using (var context = new TalentSteenEntities())
            {
                var objList = context.USP_CVDetailsByEmailId(emailId).ToList();
                if (objList.Count > 0)
                {
                    var listObj = objList.Select(x => new RecruiterCV()
                    {
                        Age = x.Age.ToString(),
                        CompanyName = x.CompanyName,
                        CountryName = x.CountryName,
                        CVId = x.CVId,
                        Designation = x.Designation,
                        FullName = x.FullName,
                        GenderName = x.GenderName,
                        MaritalStatusName = x.MaritalStatusName,
                        SkillSet = x.SkillSet
                    }).FirstOrDefault();
                    objCV = (RecruiterCV)listObj;
                }
                return objCV;
            }
        }
        [HttpPost]
        public Response CreateRecruitersJob([FromBody]JobToRecruitersModel objJobToRecruitersModelIn)
        {
            Response objResponse = new Response();
            RecruiterJob objRecruiterJob = new RecruiterJob()
            {
                JobId = objJobToRecruitersModelIn.JobId,
                RecruiterId = objJobToRecruitersModelIn.RecruiterId,
                CreatedBy = objJobToRecruitersModelIn.CreatedBy,
                CreatedOn = DateTime.Now,
                IsIntrested= objJobToRecruitersModelIn.IsIntrested
            };
            try
            {
                TalentSteenEntities obj = new TalentSteenEntities();
                obj.RecruiterJob.Add(objRecruiterJob);
                obj.SaveChanges();
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "job added successfully";

            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
    }
}
