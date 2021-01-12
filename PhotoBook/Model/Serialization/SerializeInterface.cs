using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    public interface SerializeInterface<T>
        where T : class
    {
        int SerializeObject(Serializer serializer);

        T DeserializeObject(Serializer serializer, int objectID);
    }
}
