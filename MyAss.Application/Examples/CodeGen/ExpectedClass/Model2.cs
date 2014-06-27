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

            resultModel.Add(new Generate(null, null, null, null, null));
            resultModel.Add(new Storage(null));

            return resultModel;
        }
    }
}
