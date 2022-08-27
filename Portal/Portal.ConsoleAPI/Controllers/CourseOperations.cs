using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ConsoleAPI.Controllers
{
    public enum CourseOperations
    {
        UpdateDescription = 1,
        UpdateName = 2,
        AddSkill = 3,
        DeleteSkill = 4,
        UpdateSkill = 5,
        AddMaterial = 6,
        DeleteMaterial = 7,
        UpdateMaterial = 8,
        PublishCourse = 9
    }
}
