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
    public class EmployerController : ApiController
    {
        [HttpPost]
        public Response CreateJob([FromBody]Job objJobIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            decimal? i = obj.USP_CreateJob(objJobIn.JobTitle, objJobIn.JobDescription, int.Parse(objJobIn.MinYearsofExperience), int.Parse(objJobIn.MaxYearsofExperience),
                 int.Parse(objJobIn.EducationId), int.Parse(objJobIn.MajorId), int.Parse(objJobIn.NationalityId), objJobIn.CityName, DateTime.Parse(objJobIn.StartDate), DateTime.Parse(objJobIn.EndDate)
                , int.Parse(objJobIn.EmploymentTypeId), decimal.Parse(objJobIn.RewardOffered), objJobIn.CreatedBy, int.Parse(objJobIn.JobStatusId), objJobIn.EmloyerId, int.Parse(objJobIn.JobFunctionId), objJobIn.GenderId, objJobIn.AgeFrom, objJobIn.AgeTo, int.Parse(objJobIn.SalaryFrom), int.Parse(objJobIn.SalaryTo)).FirstOrDefault();
            string res = i.ToString();
            if (decimal.Parse(i.ToString()) > 0)
            {
                #region SearchTag
                string[] tagId = objJobIn.SearchTags.Split(',').ToArray();
                if(tagId.Length>0)
                {
                    foreach(string s in tagId)
                    {
                        JobSearchTag objSearchTag = new TalentSteenAPI_V1.JobSearchTag()
                        {
                            JobId = Convert.ToInt32(i),
                            SearchTagId = int.Parse(s)
                        };
                        obj.JobSearchTag.Add(objSearchTag);
                        obj.SaveChanges();
                    }
                }

                #endregion
                #region Job Certifications
                string[] certificationId = objJobIn.Certifications.Split(',').ToArray();
                if (certificationId.Length > 0)
                {
                    foreach (string s in certificationId)
                    {
                        JobCertifications objCertifications = new TalentSteenAPI_V1.JobCertifications()
                        {
                            JobId = Convert.ToInt32(i),
                            CertificationId = int.Parse(s)
                        };
                        obj.JobCertifications.Add(objCertifications);
                        obj.SaveChanges();
                    }
                }

                #endregion
                objResponse.ReturnPK = Convert.ToInt32(i).ToString();
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Job created successfully";
            }
            else
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
        [HttpPost]
        public Response JobStatusToOpen([FromBody]Job objJobIn)
        {
            Response objResponse = new Response();
            try
            {
                TalentSteenEntities obj = new TalentSteenEntities();
                Jobs objJob = obj.Jobs.First(i => i.JobId == objJobIn.JobId);
                objJob.JobStatusId = int.Parse(objJobIn.JobStatusId);
                obj.SaveChanges();
                objResponse.ReturnPK = objJobIn.JobId.ToString();
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Job status changed successfully";
            }
            catch (Exception ex)
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
        [HttpPost]
        public EmployerJobListResponse FetchJobList([FromBody]EmployerJobListRequest objEmployerJobListRequestIn)
        {
            CommonController objCommon = new CommonController();
            TalentSteenEntities obj = new TalentSteenEntities();
            EmployerJobListResponse objResponse = new EmployerJobListResponse();
            List<USP_GetJobsByEmployer_Result> objList = new List<TalentSteenAPI_V1.USP_GetJobsByEmployer_Result>();
            objList = obj.USP_GetJobsByEmployer(objEmployerJobListRequestIn.UserId, objEmployerJobListRequestIn.EmployerId, objEmployerJobListRequestIn.Limit1, objEmployerJobListRequestIn.Limit2, objEmployerJobListRequestIn.JobStatus).ToList();
            if (objList.Count > 0)
            {
                var listObj = objList.Select(x => new Job()
                {
                    JobId = int.Parse(x.JobId.ToString()),
                    CreatedBy = int.Parse(x.CreatedBy.ToString()),
                    CreatedOn = DateTime.Parse(x.CreatedOn.ToString()),
                    Education = x.EducationName,
                    EducationMajor = x.EducationMajorName,
                    EmloyerId = x.EmployerId,
                    EmploymentTypeId = x.EmploymentTypeId.ToString(),
                    EndDate = x.EndDate.ToString(),
                    ExpairyDays = DateTime.Parse(x.EndDate.ToString()).Subtract(DateTime.Now).Days,
                    JobDescription = x.JobDescription,
                    //JobLocationId = x.JobLocationId.ToString(),
                    JobStatusId = x.JobStatusId.ToString(),
                    JobTitle = x.JobTitle,
                    JobFunctionName=x.JobFunctionName,
                    MaxYearsofExperience = x.MaximumYearsOfExperience.ToString(),
                    MinYearsofExperience = x.MinimumYearsOfExperience.ToString(),
                    NationalityId = x.CountryId.ToString(),
                    RewardOffered = x.RewardOffered.ToString(),
                    //Salary = x.SalaryRangeFrom.ToString(),
                    StartDate = x.StartDate.ToString(),
                    CreatedByUser = x.CreatedByUser.ToString(),
                    UpdatedBy = x.UpdatedBy != null ? int.Parse(x.UpdatedBy.ToString()) : 0,
                    UpdatedOn = x.UpdatedOn != null ? DateTime.Parse(x.UpdatedOn.ToString()) : DateTime.MinValue,
                    RowNo = int.Parse(x.RowNo.ToString()),
                    JobStatusList = objCommon.GetJobStatusList(int.Parse(x.JobStatusId.ToString())),
                    TotalCVCount = int.Parse(x.TotalCVCount.ToString())
                }).ToList();
                objResponse.JobListing = (List<Job>)listObj;
                objResponse.TotalRecord = int.Parse(objList[0].TotalRecord.ToString());

            }
            return objResponse;
        }
        [HttpPost]
        public JobSubmissionModel FetchJobDetails([FromBody]JobSubmissionListRequest objJobSubmissionListRequestIn)
        {
            JobSubmissionModel objOutput = new JobSubmissionModel();
            using (var context = new TalentSteenEntities())
            {
                var query = from j in context.Jobs
                            join u in context.Users on j.CreatedBy equals u.UserId
                            join js in context.JobStatus on j.JobStatusId equals js.JobStatusId
                            join jf in context.JobFunction on j.JobFunctionId equals jf.JobFunctionId
                            where j.JobId == objJobSubmissionListRequestIn.JobId
                            select new JobSubmissionModel
                            {
                                JobId = j.JobId,
                                ExpiresOn = j.EndDate.Value,
                                JobName = j.JobTitle,
                                PostedBy = u.UserName,
                                PostedOn = j.CreatedOn.Value,
                                Rewards = j.RewardOffered.ToString(),
                                LastUpdated = j.UpdatedOn.Value != null ? j.UpdatedOn.Value : j.CreatedOn.Value,
                                ExpairyDays = System.Data.Entity.DbFunctions.DiffDays(DateTime.Now, j.EndDate).Value,
                                NewResponse = 0,
                                TotalResponse = 0,
                                JobStatusName = js.JobStatusName,
                                JobFunctionName = jf.JobFunctionName
                            };
                objOutput = (JobSubmissionModel)query.SingleOrDefault();
                var query2 = from jcv in context.JobCV
                             where jcv.JobId == objJobSubmissionListRequestIn.JobId
                             group jcv by jcv.JobId into g
                             select new { TotalResponse = g.Count(), New = g.Count(), LastResponse = g.Max(t => t.SubmittedOn) };
                if (query2.Any())
                {
                    objOutput.LastResponse = query2.SingleOrDefault().LastResponse;
                    objOutput.NewResponse = query2.SingleOrDefault().New;
                    objOutput.TotalResponse = query2.SingleOrDefault().TotalResponse;
                }

                //var query3 = from jcv in context.JobCV
                //             join c in context.CV on jcv.CVId equals c.CVId
                //             join co in context.Country on c.CountryId equals co.CountryId
                //             join g in context.Gender on c.GenderId equals g.GenderId
                //             join m in context.MaritalStatus on c.MaritalStatusId equals m.MaritalStatusId
                //             where jcv.JobId == objJobSubmissionListRequestIn.JobId
                //             select new RecruiterCV
                //             {
                //                 CVId= c.CVId,
                //                 FullName=c.FullName,
                //                 CountryName = co.CountryName,
                //                 GenderName = g.GenderName,
                //                 Age = c.Age.ToString(),
                //                 MaritalStatusName = m.MaritalStatusName,
                //                 SkillSet=c.SkillSet
                //             };
                objOutput.CvList = new List<RecruiterCV>();
                List<USP_GetJobCVs_Result> objList = new List<TalentSteenAPI_V1.USP_GetJobCVs_Result>();
                objList = context.USP_GetJobCVs(objJobSubmissionListRequestIn.JobId, objJobSubmissionListRequestIn.Limit1, objJobSubmissionListRequestIn.Limit2).ToList();
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
                        RowNo = int.Parse(x.RowNo.ToString()),
                        RecruiterBrandName = x.BrandName,
                        SubmittedOn = x.SubmittedOn,
                        CVStatusId=x.CVStatusId,
                        CVStatusName=x.CVStatusName
                    }).ToList();
                    objOutput.CvList = (List<RecruiterCV>)listObj;
                    objOutput.TotalRecord = int.Parse(objList[0].TotalRecord.ToString());
                }
            }
            return objOutput;
        }
        [HttpPost]
        public Response ChangeCVStatus([FromBody]JobCVModel objJobCVModel)
        {
            Response objResponse = new Response();
            try
            {
                TalentSteenEntities obj = new TalentSteenEntities();
                JobCV objJobCV = obj.JobCV.First(i => i.JobId == objJobCVModel.JobId && i.CVId == objJobCVModel.CVId);
                objJobCV.CVStatusId = objJobCVModel.CVStatusId;
                obj.SaveChanges();
                objResponse.ReturnPK = objJobCV.JobCVId.ToString();
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Job status changed successfully";
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
