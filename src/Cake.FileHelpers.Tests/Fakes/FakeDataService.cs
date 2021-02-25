using System;
using Cake.Core;

namespace Cake.Xamarin.Tests.Fakes {
    public class FakeDataService : ICakeDataService {
        public TData Get<TData>() where TData : class {
            throw new NotImplementedException();
        }

        public void Add<TData>(TData value) where TData : class {
            throw new NotImplementedException();
        }
    }
}