To start API in dev, you should
1. `dotnet user-secrets init`
2. `dotnet user-secrets set TokenKey "some token key for JWT"`
3. `dotnet user-secrets set ConnectionStrings:PostgresConnection "some postgre connection string"`
4. `dotnet user-secrets set Cloudinary:CloudName "some cloudinary CloudName"`
5. `dotnet user-secrets set Cloudinary:ApiKey "some cloudinary ApiKey"`
6. `dotnet user-secrets set Cloudinary:ApiSecret "some cloudinary ApiSecret"`