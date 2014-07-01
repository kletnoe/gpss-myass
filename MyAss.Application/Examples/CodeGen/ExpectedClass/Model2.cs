using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.Commands;

namespace MyAss.Application.Examples.CodeGen.ExpectedClass
{
    public static class Model2
    {
        public static Model GetModel()
        {
            Model resultModel = new Model();

            if (true)
            {
                var verb = new Generate(null, null, null, null, null);
                verb.SetLabel("label1");
                resultModel.AddVerb(verb);
            }

            if (true)
            {
                var verb = new Storage(null);
                verb.SetLabel("Server");
                resultModel.AddVerb(verb);
            }

            return resultModel;
        }
    }
}
