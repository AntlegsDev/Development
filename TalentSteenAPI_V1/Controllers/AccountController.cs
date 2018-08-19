using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TalentSteenModels;
using System.Data.Entity;

namespace TalentSteenAPI_V1.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public Response CreateEmployer([FromBody]EmployerRegistration objEmployerRegistrationIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            decimal? i = obj.USP_CreateEmployer(objEmployerRegistrationIn.CompanyName, objEmployerRegistrationIn.ContactPersonName, objEmployerRegistrationIn.CorporateEmail, objEmployerRegistrationIn.ContactNumber, objEmployerRegistrationIn.Website, int.Parse(objEmployerRegistrationIn.CountryId), int.Parse(objEmployerRegistrationIn.IndustryTypeId), objEmployerRegistrationIn.UserTypeName, objEmployerRegistrationIn.UserStatusName, objEmployerRegistrationIn.EmployerInitialStatus).FirstOrDefault();
            string res = i.ToString();
            if (decimal.Parse(i.ToString()) > 0)
            {
                objResponse.IsSuccess = true;
                objResponse.SuccessMessage = "Employer registration successfully finished";
            }
            else if (decimal.Parse(i.ToString()) > -1)
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "Employer already exist";
            }
            else
            {
                objResponse.IsSuccess = false;
                objResponse.ErrorMessage = "error occured";
            }
            return objResponse;
        }

        [HttpPost]
        public Response CreateRecruiter([FromBody]RecruiterRegistration objRecruiterRegistrationIn)
        {
            //TalentSteenEntities obj = new TalentSteenEntities();
            Response objResponse = new Response();
            using (TalentSteenEntities obj = new TalentSteenAPI_V1.TalentSteenEntities())
            {
                using (var transaction = obj.Database.BeginTransaction())
                {
                    try
                    {
                        decimal? i = obj.USP_CreateRecruiter(objRecruiterRegistrationIn.FullName, DateTime.Parse(objRecruiterRegistrationIn.DateOfBirth), int.Parse(objRecruiterRegistrationIn.GenderId), int.Parse(objRecruiterRegistrationIn.LocationId), objRecruiterRegistrationIn.BrandName, objRecruiterRegistrationIn.PrimaryContactNumber, objRecruiterRegistrationIn.SecondaryContactNumber, objRecruiterRegistrationIn.PrimaryEmailId, objRecruiterRegistrationIn.SecondaryEmailId, objRecruiterRegistrationIn.OnlineProfile, objRecruiterRegistrationIn.RecruitmentHistory, int.Parse(objRecruiterRegistrationIn.CountryId), 1, objRecruiterRegistrationIn.UserTypeName, objRecruiterRegistrationIn.UserStatusName, objRecruiterRegistrationIn.RecruiterInitialStatus, int.Parse(objRecruiterRegistrationIn.YearsOfExperience)).FirstOrDefault();
                        if(objRecruiterRegistrationIn.RecruiterBankDetail!=null)
                        {
                            RecruiterBankDetails objRecruiterBankDetails = new TalentSteenAPI_V1.RecruiterBankDetails()
                            {
                                AccountNo= objRecruiterRegistrationIn.RecruiterBankDetail.AccountNo,
                                BankName = objRecruiterRegistrationIn.RecruiterBankDetail.BankName,
                                IFSCCode= objRecruiterRegistrationIn.RecruiterBankDetail.IFSCCode,
                                SwiftCode= objRecruiterRegistrationIn.RecruiterBankDetail.SwiftCode,
                                RecruiterId= Convert.ToInt32(i)
                            };
                            obj.RecruiterBankDetails.Add(objRecruiterBankDetails);
                            obj.SaveChanges();
                        }
                        if (decimal.Parse(i.ToString()) > 0)
                        {
                            if (objRecruiterRegistrationIn.RecruiterPreferredIndustries.Count() > 0)
                            {
                                foreach (string s in objRecruiterRegistrationIn.RecruiterPreferredIndustries)
                                {
                                    decimal? j = obj.USP_CreateRecruiterIndustryType(Convert.ToInt32(i), int.Parse(s)).FirstOrDefault();
                                }
                            }
                            if (objRecruiterRegistrationIn.RecruiterPreferredLocations.Count() > 0)
                            {
                                foreach (string s in objRecruiterRegistrationIn.RecruiterPreferredLocations)
                                {
                                    decimal? j = obj.USP_CreateRecruiterPrefferedLocation(Convert.ToInt32(i), int.Parse(s)).FirstOrDefault();
                                }
                            }
                            if (objRecruiterRegistrationIn.RecruiterPreferredFunctionalAreas.Count() > 0)
                            {
                                foreach (string s in objRecruiterRegistrationIn.RecruiterPreferredFunctionalAreas)
                                {
                                    decimal? j = obj.USP_CreateRecruiterFunctionalArea(Convert.ToInt32(i), int.Parse(s)).FirstOrDefault();
                                }
                            }
                            objResponse.IsSuccess = true;
                            objResponse.SuccessMessage = "Recruiter registration successfully finished";
                        }
                        else if (decimal.Parse(i.ToString()) > -1)
                        {
                            objResponse.IsSuccess = false;
                            objResponse.ErrorMessage = "Recruiter already exist";
                        }
                        else
                        {
                            objResponse.IsSuccess = false;
                            objResponse.ErrorMessage = "error occured";
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return objResponse;
        }
        [HttpPost]
        public LoggedUser Login([FromBody]LoggedUser objLoggedUserIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<USP_Login_Result> objList = new List<USP_Login_Result>();
            objList = obj.USP_Login(objLoggedUserIn.EmailId, objLoggedUserIn.Password).ToList();
            if (objList.Count > 0)
            {
                objLoggedUserIn = objList.Select(x => new LoggedUser()
                {
                    EmailId = x.EmailId,
                    EmployerId = x.EmployerId,
                    ModuleTypeId = x.ModuleType,
                    Password = x.Password,
                    RecruiterId = x.RecruiterId,
                    EntityStatusId = int.Parse(x.EntityStatusId.ToString()),
                    UserCurrentStatusId = int.Parse(x.UserStatusId.ToString()),
                    UserId = x.UserId,
                    UserTypeId = x.UserTypeId,
                    UserName = x.UserName,
                    MembershipLevel = x.MembershipLevel,
                    EmployerName = x.EmployerName,
                    RecruiterName = x.RecruiterName
                }).FirstOrDefault();
                if (objLoggedUserIn.ModuleTypeId ==((int) ModuleType.Employer).ToString())
                {
                    if(objLoggedUserIn.UserCurrentStatusId == (int)UserStatusModel.Approved && objLoggedUserIn.EntityStatusId==(int)EmployerStatusModel.Approved)
                    {
                        objLoggedUserIn.IsLoginSuccess = true;
                    }
                    else
                    {
                        objLoggedUserIn.IsLoginSuccess = false;
                        objLoggedUserIn.Message = objLoggedUserIn.EntityStatusId != (int)EmployerStatusModel.Approved ? "Employer is not approved" : (objLoggedUserIn.UserCurrentStatusId != (int)UserStatusModel.Approved ? "user is not approved" : "");
                    }
                }
                else if (objLoggedUserIn.ModuleTypeId == ((int)ModuleType.Recruiter).ToString())
                {
                    if (objLoggedUserIn.UserCurrentStatusId == (int)UserStatusModel.Approved && objLoggedUserIn.EntityStatusId == (int)RecruiterStatusModel.Approved)
                    {
                        objLoggedUserIn.IsLoginSuccess = true;
                    }
                    else
                    {
                        objLoggedUserIn.IsLoginSuccess = false;
                        objLoggedUserIn.Message = objLoggedUserIn.EntityStatusId != (int)RecruiterStatusModel.Approved ? "Recruiter is not approved" : (objLoggedUserIn.UserCurrentStatusId != (int)UserStatusModel.Approved ? "user is not approved" : "");
                    }
                }
            }
            return objLoggedUserIn;
        }
    }
}
