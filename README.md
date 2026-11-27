# CountriesRouting

A small ASP.NET Core project demonstrating **endpoint routing** with a **custom route constraint**. It exposes a list of countries and a lookup by country code, where the code format is validated by a custom `IRouteConstraint`.

## Concepts Demonstrated

- Classic `UseRouting()` / `UseEndpoints()` setup
- Registering a custom constraint via `AddRouting` and `ConstraintMap`
- Reading route values from `HttpContext.Request.RouteValues`
- Terminal middleware (`app.Run`) as a fallback when no endpoint matches

## Requirements

- .NET 6 SDK or later

## Run

```bash
dotnet run
```

The URL and port are printed in the console on startup.

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/countries` | Lists all countries as `CODE => Name` |
| GET | `/countries/{countrycode:country}` | Returns the country name for the given code |

### Examples

```
GET /countries
USA => United States of America
CND => Canada
UK => United Kingdom
IND => India
JPN => Japan

GET /countries/jpn
Japan
```

### Responses for `/countries/{countrycode}`

| Request | Result |
|---|---|
| `/countries/usa` or `/countries/USA` | `200`, `United States of America` (lookup is case-insensitive) |
| `/countries/xyz` (valid format, unknown code) | `400`, `[No country]` |
| `/countries/12` or `/countries/ABCD` (invalid format) | Constraint rejects it, no endpoint matches, fallback returns `No response` |

## Seeded Countries

| Code | Country |
|---|---|
| USA | United States of America |
| CND | Canada |
| UK | United Kingdom |
| IND | India |
| JPN | Japan |

## Project Structure

```
CountriesRouting/
├── Program.cs                          # Pipeline, routing setup, endpoints
├── Countries.cs                        # Seed data (code -> name dictionary)
└── CustomConstraints/
    └── CountryCodeConstraint.cs        # Route constraint for the "country" key
```

## How It Works

1. `CountryCodeConstraint` is registered under the name `country`:

   ```csharp
   builder.Services.AddRouting(options =>
   {
       options.ConstraintMap.Add("country", typeof(CountryCodeConstraint));
   });
   ```

2. The route uses it with `{countrycode:country}`.
3. The constraint passes only if the value matches `^[A-Za-z]{2,3}$` (2 or 3 letters).
4. The handler uppercases the code and looks it up in the dictionary.
5. If the constraint fails, routing finds no match and the terminal `app.Run(...)` middleware responds.

## Notes and Limitations

- The constraint validates **format only**, not whether the code exists. Existence is checked in the handler.
- Codes are custom, not strict ISO 3166. For example, Canada is `CND` here (ISO alpha-3 is `CAN`), and the UK is `UK` (ISO alpha-2 is `GB`).
- Data is in-memory and seeded at startup. There is no database.

## Possible Improvements

- Return `404` instead of `400` for unknown codes
- Return JSON instead of plain text
- Replace the dictionary with a database or ISO data source
- Migrate to minimal hosting style (`app.MapGet(...)` without `UseEndpoints`)
