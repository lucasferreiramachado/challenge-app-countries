# ChallengeApp.Countries

Módulo Countries publicado como pacote `ChallengeApp.Countries`.

## Desenvolvimento local no app host

```xml
<ProjectReference Include="../challenge-app-countries/Countries/Countries.csproj" />
```

Ou execute `dotnet build -p:UseLocalModules=true`.

O CI valida build, testes, cobertura e o Example MAUI. O CD publica o pacote no GitHub Packages.
