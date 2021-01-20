# NativoPlusStudio.AuthToken.Core

The core library for all NativoPlusStudio.AuthToken nuget packages. It contains the core classes and extension methods and interfaces needed to build an implementation for generating an auth token, as well as encryption and caching.

### Usage

There are multiple ways of using this library. If you want to use this library stand alone, you can use the extension method called AddTokenProviderHelper and add the necessary code to to add an implementation for IAuthTokenProvider interface.

First create your own implementation

```csharp
public class ExampleTokenProvider : BaseTokenProvider<BaseOptions>, IAuthTokenProvider
{
    public ExampleTokenProvider(
        IEncryption symmetricEncryption = null,
        IAuthTokenCacheService tokenCacheService = null,
        ILogger logger = null,
        IOptions<BaseOptions> options = null)
            :base(symmetricEncryption, tokenCacheService, logger, options)
    {

    }

    public async Task<ITokenResponse> GetTokenAsync()
    {
        //add optional code to get the token from a cache
        //add optional code to encrypt the token
        return new TokenResponse() { EncryptedToken = "thisismyencryptedtoken", ExpiryDateUtc = DateTime.UtcNow, Token = "thisismytoken", TokenType = "Bearer" };
    }
}
```

Next you can register it in a Console app or api using the AddTokenProviderHelper extension method:

```csharp
class Program
{
    public static IServiceProvider serviceProvider;
    public static IAuthTokenGenerator authTokenGenerator;
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var implementationName = "NameOfImplementation";
        services.AddTokenProviderHelper(
            protectedResourceName: implementationName,
            () =>
            {
                //necessary code to register you own implementation
                
                services.AddScoped<IAuthTokenProvider, ExampleTokenProvider>();
                //services.AddHttpClient<IAuthTokenProvider, ExampleTokenProvider>();
            }
            );

        serviceProvider = services.BuildServiceProvider();

        // you can get the service here and then use it
        authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

        var token = authTokenGenerator.GetTokenAsync(protectedResource: implementationName).GetAwaiter().GetResult();
            
        Console.WriteLine(JsonConvert.SerializeObject(token));
    }
}
```
Visit the following repositories for examples on how to use other auth token nuget packages

https://github.com/nativoplus/NativoPlusStudio.AuthToken.SymmetricEncryption
https://github.com/nativoplus/NativoPlusStudio.AuthToken.SqlServerCaching
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Ficoso
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Fis