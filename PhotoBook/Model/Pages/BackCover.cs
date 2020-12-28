using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Serialization;

namespace PhotoBook.Model.Pages
{
    public class BackCover : Page, SerializeInterface<BackCover>
    {
        public BackCover DeserializeObject()
        {
            throw new NotImplementedException();
        }

        public int SerializeObject(Serializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
