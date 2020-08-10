using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkBaseService.Interfaces
{
    public interface IRabbitListener
    {
        void Register();
        void Deregister();
    }
}
