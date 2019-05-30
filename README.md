# Prototype Command and Control Server with UI and API
This project originated as a coding challenge: given an implant communication specification, write a CnC server.

I will not share the specification, nor a sample implant.

The utility of this software merits sharing it, and is also a demonstration of various technologies working together:
- ASP.Net Core (Api Server)
- Vue.js
- Vue-router
- Modern browser fetch api
- Docker
- EntityFramework Core
- Swashbuckle
- SwaggerUI

This is nowhere near a production quality application. It was put together in less than 12 hours, but I will consider cleaning it up in future.

Also, parts of the application are currently broken, but this was originally intended for a one-time demonstration.

## Installation
Assuming this will all run on your own system:
- I used VS 2019 Community Edition as my IDE. I feel if you already don't have a copy, [then go get one](https://visualstudio.microsoft.com/)
- I also used VS Code for the Vue App, because Visual Studio doesn't get on well with what I needed (So it's optional)
- You will also need npm [get it gere](https://nodejs.org/en/)
- and Docker
- and hyper-V, because Docker (on windows, but i don't know the requirements for other systems)

- Open the solution file with visual studio.
- As long as docker-compose is set as the startup item, it should all kick off when you hit F5
**Leave all build configurations on Debug! I didn't get everything working on Release**

- The Api Server should be running on https://localhost:5001

- Then PowerShell over to the project root of the ListeningPostWebUi project (from the repo root it's: /src/ListeningPostWebUi)
- Then just call
```PowerShell
npm run dev
```

- The Web UI should now be running on http://localhost:8080 (notice the non-TLS scheme)

### Caveats
You may need to copy a certificate from the root of this repository: *.pfx to C:\tmp (yes, i realize it's tmp instead of temp).
the docker container may need to read it to start correctly. If you need to trust it or something, the password is: "thisisnotasecurepassword" (minus quotes)