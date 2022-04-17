using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace School_MIS.Models
{
    public class EnrollmentMetadata
    {
        [Display(Name = "Student Grade")]
        public Nullable<decimal> Grade { get; set; }

        [Display(Name = "Course")]
        public int CourseID { get; set; }

        [Display(Name = "Course")]
        public Course Course  { get; set; }

        [Display(Name = "Student")]
        public int StudentID { get; set; }

        [Display(Name = "Student")]
        public Student Student { get; set; }

        [Display(Name = "Lecturer")]
        public Nullable<int> LecturesId { get; set; }

        [Display(Name = "Lecturer")]
        public Lecture Lecture { get; set; }

    }

    [MetadataType(typeof(EnrollmentMetadata))]
    public partial class Enrollment { }
}