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
    public class AdminController : ApiController
    {
        [HttpPost]
        public EmployerRecruiterList GetEmployerList(ItemLimit objItemLimit)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            EmployerRecruiterList objResponse = new EmployerRecruiterList();
            EmployerList objEmployerList = new EmployerList();
            RecruiterList objRecruiterList = new RecruiterList();
            List<USP_GetEmployerList_Result> objEmpList = new List<TalentSteenAPI_V1.USP_GetEmployerList_Result>();
            List<USP_GetRecruiterList_Result> objRecList = new List<TalentSteenAPI_V1.USP_GetRecruiterList_Result>();
            int empLimit1 = 1;
            int empLimit2 = 10;
            int recLimit1 = 1;
            int recLimit2 = 10;
            if (objItemLimit.ListType == (int)ListType.EmployerList)
            {
                empLimit1 = objItemLimit.Limit1;
                empLimit2 = objItemLimit.Limit2;
            }
            else if (objItemLimit.ListType == (int)ListType.RecruiterList)
            {
                recLimit1 = objItemLimit.Limit1;
                recLimit2 = objItemLimit.Limit2;
            }
            objEmpList = obj.USP_GetEmployerList(empLimit1, empLimit2).ToList();
            List<CountryMaster> objCountryList = new List<CountryMaster>() { new CountryMaster() { CountryId = 0, CountryName = "" } };
            if (objEmpList.Count > 0)
            {
                var listObj = objEmpList.Select(x => new EmployerRegistration()
                {
                    CompanyName = x.EmployerName.ToString(),
                    ContactNumber = x.ContactNumber.ToString(),
                    ContactPersonName = x.ContactPersonName.ToString(),
                    CorporateEmail = x.CorporateEmailId.ToString(),
                    CountryId = x.CountryId.ToString(),
                    Website = x.Website.ToString(),
                    RowNo = int.Parse(x.RowNo.ToString()),
                    EmployerId = int.Parse(x.EmployerId.ToString()),
                    EmployerCurrentStatusId = int.Parse(x.EmployerStatusId.ToString())
            }).ToList();
                objEmployerList.EmployerListing = (List<EmployerRegistration>)listObj;
                objEmployerList.TotalRecord = int.Parse(objEmpList[0].TotalRecord.ToString());
            }
            objResponse.ListEmployer = objEmployerList;
            objRecList = obj.USP_GetRecruiterList(recLimit1, recLimit2).ToList();
            if (objRecList.Count > 0)
            {
                var listObj = objRecList.Select(x => new RecruiterRegistration()
                {
                    BrandName=x.BrandName,
                    AlertsAndNotification=x.AlertsAndNotification.ToString(),
                    CountryId=x.CountryId.ToString(),
                    DateOfBirth=x.DateOfBirth.ToString(),
                    FullName=x.FullName,
                    GenderId=x.GenderId.ToString(),
                    LocationId=x.LocationId.ToString(),
                    LocationName=x.LocationName,
                    OnlineProfile=x.OnlineProfile,
                    PrimaryContactNumber=x.PrimaryContactNumber,
                    PrimaryEmailId=x.PrimaryEmailId,
                    RecruitmentHistory=x.RecruitmentHistory,
                    SecondaryContactNumber=x.SecondaryContactNumber,
                    SecondaryEmailId=x.SecondaryEmailId,
                    RowNo = int.Parse(x.RowNo.ToString()),
                    RecruiterId = int.Parse(x.RecruiterId.ToString()),
                    RecruiterCurrentStatusId = int.Parse(x.RecruiterStatusId.ToString()),
                    YearsOfExperience=x.YearsOfExperience.ToString()
                    
                }).ToList();
                foreach (var s in listObj)
                {
                    var objListIndustryList = obj.USP_GetRecruiterIndustryTypeList(Convert.ToInt32(s.RecruiterId)).ToList();
                    if (objListIndustryList.Count > 0)
                    {
                        var listIndustryType = objListIndustryList.Select(x => new RecruiterIndustriesModel()
                        {
                            IndustryTypeId = x.IndustryTypeId,
                            IndustryTypeName = x.IndustryTypeName,
                            RecruiterId = x.RecruiterId,
                            RecruiterIndustryId = x.RecruiterIndustryId
                        }).ToList();
                        s.RecruiterIndustriesList = (List<RecruiterIndustriesModel>)listIndustryType;
                        s.IndustriesLists = listIndustryType.Select(i => i.IndustryTypeName).Aggregate((i, j) => i + "," + j);
                    }

                    var objListFunctionalAreaList = obj.USP_GetRecruiterFunctionalArea(Convert.ToInt32(s.RecruiterId)).ToList();
                    if (objListFunctionalAreaList.Count > 0)
                    {
                        var listFunctionalArea = objListFunctionalAreaList.Select(x => new RecruiterFunctionalAreasModel()
                        {
                           FunctionalAreaId=x.FunctionalAreaId,
                           FunctionalAreaName=x.FunctionalAreaName,
                           RecruiterId=x.RecruiterId,
                           RecruiterFunctionalAreaId=x.RecruiterFunctionalAreaId
                        }).ToList();
                        s.RecruiterFunctionalAreaList = (List<RecruiterFunctionalAreasModel>)listFunctionalArea;
                        s.FunctionalAreas = listFunctionalArea.Select(i => i.FunctionalAreaName).Aggregate((i, j) => i + "," + j);
                    }

                    var objListPreferredLocationsList = obj.USP_GetRecruiterPrefferedLocations(Convert.ToInt32(s.RecruiterId)).ToList();
                    if (objListPreferredLocationsList.Count > 0)
                    {
                        var listPreferredLocations = objListPreferredLocationsList.Select(x => new RecruiterPreferredLocationsModel()
                        {
                            LocationId = x.LocationId,
                            LocationName = x.LocationName,
                            RecruiterId = x.RecruiterId,
                            RecruiterPreferredLocationId = x.RecruiterPreferredLocationId
                        }).ToList();
                        s.RecruiterPreferredLocationList = (List<RecruiterPreferredLocationsModel>)listPreferredLocations;
                        s.PreferredLocations = listPreferredLocations.Select(i => i.LocationName).Aggregate((i, j) => i + "," + j);
                    }
                    s.AlertsAndNotificationDisplay = s.AlertsAndNotification == "1" ? "On" : "Off";
                }
                objRecruiterList.RecruiterListing = (List<RecruiterRegistration>)listObj;
                objRecruiterList.TotalRecord = int.Parse(objRecList[0].TotalRecord.ToString());
            }
            objResponse.ListRecruiter = objRecruiterList;
            return objResponse;
        }
        [HttpPost]
        public Response ActivateEmployer(EmployerRegistration objEmployerRegistration)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            decimal? i = obj.USP_EmployerActivation(objEmployerRegistration.EmployerId, objEmployerRegistration.UserStatusName, objEmployerRegistration.EmployerInitialStatus, objEmployerRegistration.UserTypeName).FirstOrDefault();
            if (decimal.Parse(i.ToString()) > 0)
            {
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Employer Activated Successfully";
            }
            else
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }
        [HttpPost]
        public Response ActivateRecruiter(RecruiterRegistration objRecruiterRegistration)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            decimal? i = obj.USP_RecruiterActivation(objRecruiterRegistration.RecruiterId, objRecruiterRegistration.UserStatusName, objRecruiterRegistration.RecruiterInitialStatus, objRecruiterRegistration.UserTypeName).FirstOrDefault();
            if (decimal.Parse(i.ToString()) > 0)
            {
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Recruiter Activated Successfully";
            }
            else
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
            List<USP_GetAllJobsForAdmin_Result> objList = new List<TalentSteenAPI_V1.USP_GetAllJobsForAdmin_Result>();
            objList = obj.USP_GetAllJobsForAdmin(objEmployerJobListRequestIn.Limit1, objEmployerJobListRequestIn.Limit2, objEmployerJobListRequestIn.JobStatus).ToList();
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
                    JobFunctionName = x.JobFunctionName,
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
        public RecruiterCVListResponse FetchRecruiterCvs([FromBody]RecruiterCVListRequest objRecruiterJobListRequestIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            RecruiterCVListResponse objResponse = new RecruiterCVListResponse();
            List<USP_GetAllCVForAdmin_Result> objList = new List<TalentSteenAPI_V1.USP_GetAllCVForAdmin_Result>();
            objList = obj.USP_GetAllCVForAdmin(objRecruiterJobListRequestIn.Limit1, objRecruiterJobListRequestIn.Limit2).ToList();
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
    }
}
