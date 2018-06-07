using Cake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.FileHelpers.Tests.Fakes
{
    class FakeDataService : ICakeDataService
    {
        public void Add<TData>(TData value) where TData : class
        {
            throw new NotImplementedException();
        }

        public TData Get<TData>() where TData : class
        {
            throw new NotImplementedException();
        }
    }
}
