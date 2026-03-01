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

It uses the built‑in `HttpClient` and the [YARP Reverse Proxy](https://microsoft.github.io/reverse-proxy/)
package for simplicity.

> ⚠️ This is not production‑grade. There’s no health checking, TLS support, or
> advanced routing. It’s purely for educational purposes.
> Will be working on it in the future and make it more Prod ready

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Windows, macOS or Linux
- A few simple backend services (e.g. `http://localhost:5001`, `http://localhost:5002`)

## 📦 Getting Started

1. Clone the repo:

   ```bash
   git clone https://github.com/yourname/simple-csharp-loadbalancer.git
   cd simple-csharp-loadbalancer