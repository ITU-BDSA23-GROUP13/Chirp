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
![Onion architecture as a diagram.](./images/onion_architecture.png)

Each layer only depends on the layer it encapsulates, i.e. Chirp.Infrastructure depends on Chirp.Core but not Chirp.Web.

The layer Chirp.Core contains interfaces, in yellow, and DTOs, in red. The interfaces describe the communication between the database and the application. The DTOs (data transfer models) are objects that are used to send data between the layers and consists of a common set data that is used by both Chirp.Web and Chirp.Infrastructure.

The layer Chirp.Infrastructure implements the interfaces from Chirp.Core. _Chirp!_ uses Entity Framework Core to manage writing the queries for the database, which helps keep Chirp.Infrastructure database agnostic. It also describes how the database should be modelled.

The layer Chirp.Web handles connecting to the database, reacts to requests, and displays the web pages. Chirp.Web uses Chirp.Infrastructure through the interfaces and DTOs from Chirp.Core. _Chirp!_ uses ASP.NET Core Razor Pages to help create the web application.

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment
![Build and Test workflow as an Activity diagram.](./images/build_and_test.png)
![Build and Deploy workflow as an Activity diagram.](./images/build_and_deploy.png)
![Publish and Release as an Activity diagram.](./images/publish_and_release.png)

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
_Chirp!_ uses the MIT license. This license was chosen because it is a very permissive license, which we don't mind as _Chirp!_ was only developed for educational purposes. It is also very short and simple to understand.

## LLMs, ChatGPT, CoPilot, and others
ChatGPT and GitHub Copilot has been used the authors of _Chirp!_.

The use of ChatGPT has been _very_ minimal, and no code was directly copied from ChatGPT. It has been used by some authors to help understand some concepts or error messages. It has also been used to answer more creative questions such as what kinds of reactions would fit the theme of _Chirp!_ if we wanted to add reactions to Cheeps.

GitHub Copilot has also been used by some authors, but it has been used exclusively as a tool to write code/boilerplate _faster_. Some editors allow you to disable Copilot from automatically suggesting code, and instead use a keybinding to manually make Copilot come with a one-time suggestion. This has the benefit of making Copilot not show suggestions when it is not needed, which is the vast majority of the time we spend coding. Though, we do find Copilot to be very useful when we write boilerplate/repeating/obvious code, and in these cases Copilot can be manually enabled to generate the code. In other words, Copilot has primarialy been used to generate code that we already intended to write, and not to generate new code that we didn't understand.

