using NetArchTest.Rules;
using System.Reflection;

namespace CleanArchitecture.Tests
{
    public class ArchitectureDependancy
    {
        private readonly Assembly DomainAssembly = LoadAssembly("Domain");
        private readonly Assembly ApplicationAssembly = LoadAssembly("Application");
        private readonly Assembly InfrastructureAssembly = LoadAssembly("Infrastructure");

        [Fact]
        public void Domain_ShouldNotReferenceAnyOtherLayer_ThatIsNotShared()
        {
            var result = Types.InAssembly(DomainAssembly)
                .ShouldNot()
                .HaveDependencyOnAny("Application", "Infrastructure", "TABP")
                .GetResult();
            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void Application_ShouldNotReferenceAnyOtherLayer_ThatIsNotShared()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .ShouldNot()
                .HaveDependencyOnAny("Infrastructure", "TABP")
                .GetResult();
            Assert.True(result.IsSuccessful);
        }
        [Fact]
        public void Infrastructure_ShouldNotReferenceAnyOtherLayer_ThatIsNotShared()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .ShouldNot()
                .HaveDependencyOnAny("TABP")
                .GetResult();
            Assert.True(result.IsSuccessful);
        }

        private static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                throw new System.Exception($"Assembly named : {assemblyName} not found");
            }
        }
    }
}