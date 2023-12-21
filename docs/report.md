---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `13`
author:
- "Marcus Alsted Wegmann <maaw@itu.dk>"
- "Daniel Ahmadi <daah@itu.dk>"
- "Christopher Robin Heldgaard <chre@itu.dk>"
- "Connie Petersson <cope@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model
The following entity-relation-diagram illustrates _Chirp!_'s domain model.
![Data model as an ER-diagram.](./images/er_diagram.png)

Each Cheep stores its id, some text, a timestamp that denotes when it was posted, and its author's id.

Each Author (AspNetUser) stores their id, username, email, password hash, whether they have two-factor-authentication enables, aswell the above mentioned. Author's can also follow each other in a many-to-many relation.

The AspNetUser, AspNetUserTokens, and AspNetUserLogins comes from ASP.NET Core Identity that _Chirp!_ uses to manage its users (i.e. Authors). In reality, each AspNetUser also stores additional attributes that are not used directly by _Chirp!_. These are normalized username and email, concurrency and sercurity stamps (used for e.g. password-resetting), phone number (_Chirp!_ doesn't collect phone numbers), lockout information, and an access failure count.
The AspNetUserTokens stores tokens such as two-factor-authentication keys and recovery codes, and AspNetUserLogins stores third-party login provider information, e.g. Github OAuth.

## Architecture — In the small
The following diagram illustrates _Chirp!_ overall architecture.
![Onion architecture as a diagram.](.images/onion_architecture.png)

Each layer only depends on the layer it encapsulates, i.e. Chirp.Infrastructure depends on Chirp.Core but not Chirp.Web.

The layer Chirp.Core contains interfaces, in yellow, and DTOs, in red. The interfaces describe the communication between the database and the application. The DTOs (data transfer models) are objects that are used to send data between the layers and consists of a common set data that is used by both Chirp.Web and Chirp.Infrastructure.

The layer Chirp.Infrastructure implements the interfaces from Chirp.Core. _Chirp!_ uses Entity Framework Core to manage writing the queries for the database, which helps keep Chirp.Infrastructure database agnostic. It also describes how the database should be modelled.

The layer Chirp.Web handles connecting to the database, reacts to requests, and displays the web pages. Chirp.Web uses Chirp.Infrastructure through the interfaces and DTOs from Chirp.Core. _Chirp!_ uses ASP.NET Core Razor Pages to help create the web application.

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
