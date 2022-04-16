using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace School_MIS.Models
{
    public class CourseMetaData
    {
        [StringLength(50)]
        public string Title { get; set; }

        [Range(1,8)]
        public int Credits { get; set; }
    }

    [MetadataType(typeof(CourseMetaData))]
    public partial class Course { }
}