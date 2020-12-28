using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Serialization;

namespace PhotoBook.Model.Backgrounds
{
    public abstract class Background : SerializeInterface<Background>
    {
        public Background DeserializeObject()
        {
            throw new NotImplementedException();
        }

        public int SerializeObject(Serializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
