//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TalentSteenAPI_V1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Jobs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Jobs()
        {
            this.JobSearchTag = new HashSet<JobSearchTag>();
            this.RecruiterJob = new HashSet<RecruiterJob>();
            this.JobCertifications = new HashSet<JobCertifications>();
            this.JobCV = new HashSet<JobCV>();
        }
    
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public int MinimumYearsOfExperience { get; set; }
        public int MaximumYearsOfExperience { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EmploymentTypeId { get; set; }
        public Nullable<decimal> RewardOffered { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> JobStatusId { get; set; }
        public int EmployerId { get; set; }
        public Nullable<int> JobFunctionId { get; set; }
        public Nullable<int> EducationId { get; set; }
        public Nullable<int> EducationMajorId { get; set; }
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> AgeFrom { get; set; }
        public Nullable<int> AgeTo { get; set; }
        public Nullable<int> SalaryRangeFrom { get; set; }
        public Nullable<int> SalaryRangeTo { get; set; }
        public Nullable<int> CityId { get; set; }
    
        public virtual City City { get; set; }
        public virtual EducationMajorMaster EducationMajorMaster { get; set; }
        public virtual EducationMaster EducationMaster { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual JobFunction JobFunction { get; set; }
        public virtual JobStatus JobStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobSearchTag> JobSearchTag { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruiterJob> RecruiterJob { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobCertifications> JobCertifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobCV> JobCV { get; set; }
    }
}
