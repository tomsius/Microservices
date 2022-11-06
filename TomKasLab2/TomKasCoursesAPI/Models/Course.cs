using System.ComponentModel.DataAnnotations.Schema;

namespace TomKasCoursesAPI.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CourseID { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }

}
