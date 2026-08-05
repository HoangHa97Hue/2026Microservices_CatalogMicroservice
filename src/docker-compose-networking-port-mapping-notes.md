Filename (English): docker-compose-networking-port-mapping-notes.md

# Docker Compose Networking & Port Mapping Notes

## Key idea: Host vs Container networks are different
- The **host machine** (your laptop) and each **container** have separate network namespaces.
- `localhost` means:
  - On **host**: your host machine
  - Inside a **container**: that container itself (NOT the host, NOT other containers)

## Port mapping syntax
In Compose:

```yml
ports:
  - "6062:8081"
6062 = host port (reachable from your machine via localhost:6062)
8081 = container port (the port the app listens on inside the container)
Docker performs port forwarding/NAT:

HOST:6062 -> CONTAINER:8081
So from host you call:

https://localhost:6062 and Docker routes it to the container’s 8081.
Container-to-container communication (within the same Compose network)
Containers in the same Compose project can reach each other by service name (built-in DNS).
They should use container port, not host-mapped port.
Example:

discount.grpc listens on 8081 inside the container
Other containers call:
https://discount.grpc:8081
Why not localhost:6062 from another container?
Because inside container B, localhost = container B. Host port 6062 exists on the host network, not on container B.

Rule of thumb
From host → use host port (localhost:<HOST_PORT>)
From container → use service name + container port (http(s)://<service>:<CONTAINER_PORT>)
Practical example (from the screenshots)
Local (host) config might look like:
GrpcSettings:DiscountUrl = https://localhost:5052 (host-access scenario)
Docker Compose override for inter-container calls should be:
GrpcSettings__DiscountUrl = https://discount.grpc:8081
Notes
ports: is mainly for exposing services to host/local development (or external access).
For internal-only communication, containers don’t need ports:; they can talk via the internal network (optionally document with expose:).


###############################################################################
Docker sets up a forwarding rule like:

HOST:6062 -> (NAT/forward) -> CONTAINER:8081
So from your host machine you can call:

https://localhost:6062
You don’t need to call 8081 from the host, because Docker already maps host port 6062 to the container’s internal port 8081.

Key takeaway

6062 is for host access
8081 is where the app actually listens inside the container
3) Why another container cannot use https://localhost:6062
Because 6062 exists on the host network, not inside other containers.

If container B calls:

https://localhost:6062
then localhost refers to container B, so it is effectively calling:

container B:6062 (itself)
That’s why it fails (unless container B also exposes 6062 internally, which is unrelated).

“Can a container call the host port at all?”
Sometimes yes, via special hostnames (Docker Desktop):

host.docker.internal
But this is a detour and generally not the standard approach for service-to-service communication in Compose. The recommended way is container-to-container networking.

4) Correct way: container-to-container calls use service name + container port
Docker Compose creates an internal network and DNS:

each service name becomes a hostname (e.g. discount.grpc)
So from basket.api container, the correct address is:

https://discount.grpc:8081
Why you must specify 8081:

A hostname can serve multiple ports.
If you don’t specify a port, clients assume defaults:
HTTP default: 80
HTTPS default: 443
But in your case the service listens on 8080/8081 inside the container.
Rule of thumb

From host: use localhost:<HOST_PORT>
From container: use <service-name>:<CONTAINER_PORT>
5) Practical mapping in this project
discount.grpc
Inside container:
HTTP: 8080 (ASPNETCORE_HTTP_PORTS=8080)
HTTPS: 8081 (ASPNETCORE_HTTPS_PORTS=8081)
Published to host for local access:
Host 6002 -> Container 8080
Host 6062 -> Container 8081
basket.api -> discount.grpc (inside Docker)
Use:

GrpcSettings__DiscountUrl = "https://discount.grpc:8081"
basket.api -> discount.grpc (running on host)
If discount.grpc is exposed to host as 5052 (example from screenshot), then:

GrpcSettings:DiscountUrl = "https://localhost:5052"
(Host values depend on your actual published host ports.)

6) Quick checklist (common mistakes)
Calling localhost:<host-port> from inside a container (wrong target)
Using host-mapped port for internal service calls (should use container port)
Forgetting that HTTPS default is 443 (so https://discount.grpc won’t hit 8081)
Mixing local appsettings (localhost) with docker override (service name)