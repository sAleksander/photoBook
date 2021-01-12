using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    interface ObjectDataRetrieverInterface<T>
        where T : class
    {
        T Get<T>(string propertyName);
    }
}
