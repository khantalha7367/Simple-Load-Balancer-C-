# Simple C# Load Balancer

This repository contains a minimal **load balancer** implemented in C# using .NET 8.  
It’s designed as a learning/demo project and demonstrates how you can route
incoming HTTP requests to a pool of backend servers.

## 🚀 Overview

The load balancer:

- Listens for HTTP requests on a configurable port.
- Maintains a list of backend endpoints.
- Uses a **round‑robin** strategy to forward each request to the next server.
- Relays responses back to the original client.

- In the latest commit further features such as Jwt Authentication
- Serilog logging service
- Security Headers
- Rate Limiters

It uses the built‑in `HttpClient` and the [YARP Reverse Proxy](https://microsoft.github.io/reverse-proxy/)
package for simplicity.

It also uses the following packages (Name, Version):

> Microsoft.AspNetCore.Authentication.JwtBearer      8.0.0       
> NetEscapades.AspNetCore.SecurityHeaders            1.3.1       
> Serilog.Extensions.Hosting                         8.0.0       
> System.IdentityModel.Tokens.Jwt                    8.16.0      
> Yarp.ReverseProxy                                  2.3.0       


## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Windows, macOS or Linux
- A few simple backend services (e.g. `http://localhost:5001`, `http://localhost:5002`)

## 📦 Getting Started

1. Clone the repo:

   ```bash
   git clone https://github.com/yourname/simple-csharp-loadbalancer.git
   cd simple-csharp-loadbalancer
