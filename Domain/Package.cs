using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class Package<T>
    {
        public User User { get; set; }
        //public string Message { get; set; }
        public T Content { get; set; }

        public Package(User user, T content)
        {
            this.User = user;
            this.Content = content;
        }
    }
}
