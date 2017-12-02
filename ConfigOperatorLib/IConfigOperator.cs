using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigOperatorLib
{
   public  interface IConfigOperator
    {
        string getProperty(string propertyName);

        void SetProperty(string propertyName, string propertyValue);
    }
}
