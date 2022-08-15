# rapids_rivers_in_csharp

Copyright (c) 2022 by Fred George  
@author Fred George  fredgeorge@acm.org  
Licensed under the MIT License; see LICENSE file in root.

A C# implementation of Rapids/Rivers framework using RabbitMQ for the event bus

## Summary

This framework models the Rapids/Rivers/Pond metaphor first proposed by Fred George.
It strives to present a relatively simple choreography scheme to allow tiny MicroServices
to interact with minimal coupling. This is design port of the Kotlin version,
rapids_rivers_in_kotlin.

Originally formulated for a workshop conference in Bergen, Norway, and it has been re-implemented
several times, including NAV (Norwegian Welfare Association) in 2019 and Orn Software in 2022 (in C#).

## Dependencies

This project is built with:

- IntelliJ Jetbrains Rider 2022.1.1 
- .NET 6.0
- xUnit
- and other libraries for JSON and RabbitMQ

So open up the project, and let the dependencies resolve themselves. Execute the tests
to ensure everything is working correctly.

Now you are ready to run the sample MicroServices.

## Execution

For any event bus, an implementation of the RapidsConnection interface is required.
A RabbitMQ implementation is included, using RabbitMQ in a pub/sub mode.

To run the sample MicroServices, bring up a RabbitMQ implementation, either natively 
or in a Docker container. Or share an existing instance running somewhere else. 
RabbitMQ with the Management console option is recommended so you can view status
and statistics on the event bus.

Then start up the Monitor service by passing the IP address and the RabbitMQ 
port (usually 5672) as _strings_ as input parameters. You should see a simple prompt.

Next, start up the RentalNeed service with the same parameters. It should generate
traffic that the Monitor shows on the event bus.

Now you are ready to start developing your own services.
