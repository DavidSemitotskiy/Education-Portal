﻿using Portal.Domain.BaseModels;

namespace Portal.Domain.Models
{
    public abstract class Material : Entity
    {
        public List<Course> Courses { get; set; }
    }
}
