using Cake.Core.Configuration;

namespace Cake.Xamarin.Tests.Fakes
{
  internal sealed class FakeCakeConfiguration : ICakeConfiguration
  {
    public string GetValue(string key) => string.Empty;
  }
}
