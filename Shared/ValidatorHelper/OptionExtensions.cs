using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Shared.ValidatorHelper
{
    public static class OptionExtensions
    {
        public static OptionsBuilder<TOptions> FluentValidation<TOptions>(
          this OptionsBuilder<TOptions> builder)
          where TOptions : class
        {
            builder.Services.AddSingleton<IValidateOptions<TOptions>, FluentValidateOptions<TOptions>>();

            return builder;
        }
    }
}
