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
    public class CommonController : ApiController
    {
        /// <summary>
        /// get all country list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CountryMaster> GetCountryList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<CountryMaster> objList = new List<CountryMaster>();
            CountryMaster objCountryMaster = new CountryMaster();
            List<USP_GetCountryList_Result> obj2 = new List<USP_GetCountryList_Result>();
            obj2 = obj.USP_GetCountryList(0).ToList();
            foreach (USP_GetCountryList_Result objNew in obj2)
            {
                objCountryMaster = new CountryMaster();
                objCountryMaster.CountryId = objNew.CountryId;
                objCountryMaster.CountryName = objNew.CountryName;
                objCountryMaster.CountryCode = objNew.CountryCode;
                objList.Add(objCountryMaster);
            }
            return objList;
        }
        /// <summary>
        /// get country details by passing id
        /// </summary>
        /// <param name="objCountryMasterIn"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CountryMaster> GetCountryDetails([FromBody]CountryMaster objCountryMasterIn)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<CountryMaster> objList = new List<CountryMaster>();
            CountryMaster objCountryMaster = new CountryMaster();
            List<USP_GetCountryList_Result> obj2 = new List<USP_GetCountryList_Result>();
            obj2 = obj.USP_GetCountryList(objCountryMasterIn.CountryId).ToList();
            foreach (USP_GetCountryList_Result objNew in obj2)
            {
                objCountryMaster = new CountryMaster();
                objCountryMaster.CountryId = objNew.CountryId;
                objCountryMaster.CountryName = objNew.CountryName;
                objCountryMaster.CountryCode = objNew.CountryCode;
                objList.Add(objCountryMaster);
            }
            //objList = (CountryList)obj1;
            return objList;
        }

        [HttpGet]
        public List<Location> GetLocationList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<Location> objList = new List<Location>();
            Location objLocation = new Location();
            List<USP_GetLocationList_Result> obj2 = new List<USP_GetLocationList_Result>();
            obj2 = obj.USP_GetLocationList(0).ToList();
            foreach (USP_GetLocationList_Result objNew in obj2)
            {
                objLocation = new Location();
                objLocation.LocationId = objNew.LocationId;
                objLocation.LocationName = objNew.LocationName;
                objList.Add(objLocation);
            }
            return objList;
        }

        [HttpGet]
        public List<EmploymentType> GetEmploymentTypeList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<EmploymentType> objList = new List<EmploymentType>();
            EmploymentType objEmploymentType = new EmploymentType();
            List<USP_GetEmploymentTypeList_Result> obj2 = new List<USP_GetEmploymentTypeList_Result>();
            obj2 = obj.USP_GetEmploymentTypeList(0).ToList();
            foreach (USP_GetEmploymentTypeList_Result objNew in obj2)
            {
                objEmploymentType = new EmploymentType();
                objEmploymentType.EmploymentTypeId = objNew.EmploymentTypeId;
                objEmploymentType.EmploymentTypeName = objNew.EmploymentTypeName;
                objList.Add(objEmploymentType);
            }
            return objList;
        }

        [HttpGet]
        public List<IndustryType> GetIndustryTypeList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<IndustryType> objList = new List<IndustryType>();
            IndustryType objIndustryType = new IndustryType();
            List<USP_GetIndustryTypeList_Result> obj2 = new List<USP_GetIndustryTypeList_Result>();
            obj2 = obj.USP_GetIndustryTypeList(0).ToList();
            foreach (USP_GetIndustryTypeList_Result objNew in obj2)
            {
                objIndustryType = new IndustryType();
                objIndustryType.IndustryTypeId = objNew.IndustryTypeId;
                objIndustryType.IndustryTypeName = objNew.IndustryTypeName;
                objList.Add(objIndustryType);
            }
            return objList;
        }

        [HttpGet]
        public List<FunctionalArea> GetFunctionalAreaList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<FunctionalArea> objList = new List<FunctionalArea>();
            FunctionalArea objFunctionalArea = new FunctionalArea();
            List<USP_GetFunctionalAreaList_Result> obj2 = new List<USP_GetFunctionalAreaList_Result>();
            obj2 = obj.USP_GetFunctionalAreaList(0).ToList();
            foreach (USP_GetFunctionalAreaList_Result objNew in obj2)
            {
                objFunctionalArea = new FunctionalArea();
                objFunctionalArea.FunctionalAreaId = objNew.FunctionalAreaId;
                objFunctionalArea.FunctionalAreaName = objNew.FunctionalAreaName;
                objList.Add(objFunctionalArea);
            }
            return objList;
        }

        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetGenderList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<USP_GetGenderList_Result> obj2 = new List<USP_GetGenderList_Result>();
            obj2 = obj.USP_GetGenderList(0).ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.GenderName.ToString(),
                Value = x.GenderId.ToString()
            });
            return listItem;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetGenderListWithBoth()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<USP_GetGenderList_Result> obj2 = new List<USP_GetGenderList_Result>();
            obj2 = obj.USP_GetGenderList(0).ToList();
            obj2.Insert(0, new USP_GetGenderList_Result() { GenderId = 0, GenderName = "Any" });
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.GenderName.ToString(),
                Value = x.GenderId.ToString()
            });
            return listItem;
        }

        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetJobStatusList(int jobStatusId)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<JobStatus> obj2 = new List<JobStatus>();
            obj2 = obj.JobStatus.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.JobStatusName.ToString(),
                Value = x.JobStatusId.ToString(),
                Selected = (jobStatusId == int.Parse(x.JobStatusId.ToString()) ? true : false)
            });
            return listItem;
        }
        [HttpGet]
        public List<CVStatusModel> GetCVStatusList(int rejectedStatusId)
        {
            List<CVStatusModel> objResult = new List<CVStatusModel>();
            using (var context = new TalentSteenEntities())
            {
                var query = from c in context.CVStatus
                            where c.CVStatusId != rejectedStatusId
                            select new CVStatusModel
                            {
                                CVStatusId = c.CVStatusId,
                                CVStatusName = c.CVStatusName
                            };
                objResult = (List<CVStatusModel>)query.ToList();
            }
            return objResult;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetJobFunctionsList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<JobFunction> obj2 = new List<JobFunction>();
            obj2 = obj.JobFunction.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.JobFunctionName.ToString(),
                Value = x.JobFunctionId.ToString()
            });
            return listItem;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetEducationMasterList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<EducationMaster> obj2 = new List<EducationMaster>();
            obj2 = obj.EducationMaster.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.EducationName.ToString(),
                Value = x.EducationId.ToString()
            });
            return listItem;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetEducationMajorMasterList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<EducationMajorMaster> obj2 = new List<EducationMajorMaster>();
            obj2 = obj.EducationMajorMaster.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.EducationMajorName.ToString(),
                Value = x.EducationMajorId.ToString()
            });
            return listItem;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetCertificationMasterList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<CertificationMaster> obj2 = new List<CertificationMaster>();
            obj2 = obj.CertificationMaster.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.CertificationName.ToString(),
                Value = x.CertificationId.ToString()
            });
            return listItem;
        }
        [HttpPost]
        public List<string> GetCityList([FromBody]string word)
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            List<string> listItem=new List<string>();
            List<USP_GetCityDetails_Result> obj2 = new List<USP_GetCityDetails_Result>();
            obj2 = obj.USP_GetCityDetails(word).ToList();
            if(obj2!=null)
            {
                foreach(USP_GetCityDetails_Result s in obj2)
                {
                    listItem.Add(s.CityName);
                }
            }
            return listItem;
        }
        [HttpGet]
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSearchTagList()
        {
            TalentSteenEntities obj = new TalentSteenEntities();
            IEnumerable<System.Web.Mvc.SelectListItem> listItem;
            List<SearchTagMaster> obj2 = new List<SearchTagMaster>();
            obj2 = obj.SearchTagMaster.ToList();
            listItem = obj2.Select(x => new System.Web.Mvc.SelectListItem()
            {
                Text = x.SearchTag.ToString(),
                Value = x.SearchTagMasterId.ToString()
            });
            return listItem;
        }
    }
}
