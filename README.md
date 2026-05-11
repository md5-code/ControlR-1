# Internal Remote Management Tool

A private internal build maintained by **md5-hash**.

This is a private fork. It is not affiliated with, endorsed by, or supported by the upstream project authors. Public issues, support requests, and discussions are not accepted here.

## Status

Internal use only. Distribution outside of authorised deployments is not permitted.

## License

This software incorporates third-party code distributed under the MIT License. See [LICENSE.txt](./LICENSE.txt) for the full notice and required copyright attribution.

## Build / Deploy

Build and deployment instructions are maintained internally. The docker-compose files in [`./docker-compose`](./docker-compose) reference a private container image; replace the `image:` tag with the registry path used in your environment before running.

## Logs

Agent and desktop client logs are written to:

- **Windows**: `C:\ProgramData\PyTrain\{hostname}\Logs\`
- **macOS / Linux (root)**: `/var/log/pytrain/{hostname}/`
- **macOS / Linux (user)**: `~/.pytrain/logs/{hostname}/`
