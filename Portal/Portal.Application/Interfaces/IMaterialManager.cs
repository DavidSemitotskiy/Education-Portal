using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Interfaces
{
    public interface IMaterialManager
    {
        IMaterialRepository MaterialRepository { get; set; }

        void AddMaterial(Material material);

        void DeleteMaterial(Material material);
    }
}
